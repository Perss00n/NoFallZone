using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Services.Implementations;
public class OrderService : IOrderService
{
    private readonly NoFallZoneContext db;

    public OrderService(NoFallZoneContext context)
    {
        db = context;
    }

    public async Task<bool> PlaceOrderAsync(int shippingOptionId, int paymentMethodId)
    {
        if (!Session.IsLoggedIn)
        {
            OutputHelper.ShowError("You must be logged in to place an order!");
            return false;
        }

        if (Session.Cart.Count == 0)
        {
            OutputHelper.ShowError("Your cart is empty!");
            return false;
        }

        try
        {
            var shipping = await db.ShippingOptions.FirstOrDefaultAsync(s => s.Id == shippingOptionId);
            if (shipping == null)
            {
                OutputHelper.ShowError("Invalid shipping option!");
                return false;
            }

            var payment = await db.PaymentOptions.FirstOrDefaultAsync(p => p.Id == paymentMethodId);
            if (payment == null)
            {
                OutputHelper.ShowError("Invalid payment option!");
                return false;
            }

            foreach (var item in Session.Cart)
            {
                var dbProduct = await db.Products.FirstOrDefaultAsync(p => p.Id == item.Product.Id);
                if (dbProduct == null || dbProduct.Stock < item.Quantity)
                {
                    OutputHelper.ShowError($"Not enough stock for {item.Product.Name}. Available: {dbProduct?.Stock}");
                    return false;
                }
            }

            var order = new Order
            {
                CustomerId = Session.LoggedInUser!.Id,
                OrderDate = DateTime.Now,
                ShippingOptionId = shippingOptionId,
                ShippingCost = shipping.Price,
                PaymentOptionId = paymentMethodId,
                PaymentOptionName = payment.Name ?? "Unknown",
                TotalPrice = Session.GetCartTotal() + shipping.Price + (payment.Fee),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in Session.Cart)
            {
                var product = await db.Products.FirstAsync(p => p.Id == item.Product.Id);

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    PricePerUnit = product.Price,
                });

                product.Stock -= item.Quantity;
            }

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            Session.Cart.Clear();
            await ShowReceiptAsync(order);
            await LogHelper.LogAsync(db, "Purchase", $"OrderId: {order.Id}, Total: {order.TotalPrice:C}");
            return true;
        }
        catch (Exception ex)
        {
            OutputHelper.ShowError("An error occurred while placing the order!" +
                (Session.IsAdmin ? $"\n\nDetails: {ex.Message}" : " Please contact an administrator if this issue persists."));
            return false;
        }
    }

    public void ShowOrderPreview(ShippingOption shipping, PaymentOption payment)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var lines = new List<string>
    {
        $"Customer:       {Session.LoggedInUser?.Username ?? "Unknown"}",
        $"Shipping:       {shipping.Name} ({shipping.Price:C})",
        $"Payment Method: {payment.Name} (Fee: {payment.Fee:C})",
        $"--------------------------------------------------"
    };

        foreach (var item in Session.Cart)
        {
            var product = item.Product;
            decimal lineTotal = product.Price * item.Quantity;

            lines.Add($"{item.Quantity} x {product.Name.PadRight(25)} ({product.Price,6:C}) = {lineTotal,8:C}");
        }

        decimal subtotal = Session.GetCartTotal();
        decimal shippingCost = shipping.Price;
        decimal paymentFee = payment.Fee;
        decimal total = subtotal + shippingCost + paymentFee;

        lines.Add($"--------------------------------------------------");
        lines.Add($"Subtotal:     {subtotal,10:C}");
        lines.Add($"Shipping:     {shippingCost,10:C}");
        lines.Add($"Payment Fee:  {paymentFee,10:C}");
        lines.Add($"--------------------------------------------------");
        lines.Add($"TOTAL:        {total,10:C}");

        GUI.DrawWindow("Order Summary (Preview)", 1, 10, lines, 80);
    }

    private async Task ShowReceiptAsync(Order order)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var customerName = Session.LoggedInUser?.Username ?? "Unknown";
        var paymentName = order.PaymentOption?.Name ?? "N/A";
        var paymentFee = order.PaymentOption?.Fee ?? 0;
        var shipping = await db.ShippingOptions.FirstOrDefaultAsync(s => s.Id == order.ShippingOptionId);
        var shippingName = shipping?.Name ?? "Unknown";
        var shippingCost = shipping?.Price ?? order.ShippingCost;

        var lines = new List<string>
    {
        $"Thank you for your order, {customerName}!",
        $"Order Date:    {order.OrderDate:G}",
        $"Payment:       {paymentName} (Fee: {paymentFee:C})",
        $"Shipping:      {shippingCost:C} via {shippingName}",
        $"--------------------------------------------------"
    };

        foreach (var item in order.OrderItems)
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
            string productName = product?.Name ?? "Unknown Product";
            decimal lineTotal = item.PricePerUnit * item.Quantity;

            lines.Add($"{item.Quantity} x {productName.PadRight(25)} ({item.PricePerUnit,6:C}) = {lineTotal,8:C}");
        }

        lines.Add($"--------------------------------------------------");
        lines.Add($"TOTAL:         {order.TotalPrice,10:C}");

        GUI.DrawWindow("Order Receipt", 1, 10, lines, 80);
    }
}