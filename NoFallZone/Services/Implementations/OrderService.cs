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

    public bool PlaceOrder(int shippingOptionId, int paymentMethodId, out string message)
    {
        message = string.Empty;

        if (!Session.IsLoggedIn)
        {
            message = "You must be logged in to place an order!";
            return false;
        }

        if (Session.Cart.Count == 0)
        {
            message = "Your cart is empty!";
            return false;
        }

        try
        {
            var shipping = db.ShippingOptions.FirstOrDefault(s => s.Id == shippingOptionId);
            if (shipping == null)
            {
                message = "Invalid shipping option!";
                return false;
            }

            var payment = db.PaymentOptions.FirstOrDefault(p => p.Id == paymentMethodId);
            if (payment == null)
            {
                message = "Invalid payment option!";
                return false;
            }

            foreach (var item in Session.Cart)
            {
                var dbProduct = db.Products.FirstOrDefault(p => p.Id == item.Product.Id);
                if (dbProduct == null || dbProduct.Stock < item.Quantity)
                {
                    message = $"Not enough stock for {item.Product.Name}. Available: {dbProduct?.Stock ?? 0}";
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
                TotalPrice = Session.GetCartTotal() + shipping.Price + (payment.Fee ?? 0),
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in Session.Cart)
            {
                var product = db.Products.First(p => p.Id == item.Product.Id);

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    PricePerUnit = product.Price,
                });

                product.Stock -= item.Quantity;
            }

            db.Orders.Add(order);
            db.SaveChanges();

            Session.Cart.Clear();
            ShowReceipt(order);

            message = "Order completed successfully!";
            return true;
        }
        catch (Exception ex)
        {
            message = "An error occurred while placing the order! Please contact a administrator if this issue persists.";

            if (Session.IsAdmin)
            {
                message += $"\n\nDetails: {ex.Message}";
                if (ex.InnerException != null)
                    message += $"\n\n{ex.InnerException.Message}";
            }

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
        $"Payment Method: {payment.Name} (Fee: {payment.Fee.GetValueOrDefault():C})",
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
        decimal paymentFee = payment.Fee.GetValueOrDefault();
        decimal total = subtotal + shippingCost + paymentFee;

        lines.Add($"--------------------------------------------------");
        lines.Add($"Subtotal:     {subtotal,10:C}");
        lines.Add($"Shipping:     {shippingCost,10:C}");
        lines.Add($"Payment Fee:  {paymentFee,10:C}");
        lines.Add($"--------------------------------------------------");
        lines.Add($"TOTAL:        {total,10:C}");

        GUI.DrawWindow("Order Summary (Preview)", 1, 10, lines, 80);
    }

    private void ShowReceipt(Order order)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var customerName = Session.LoggedInUser?.Username ?? "Unknown";
        var paymentName = order.PaymentOption?.Name ?? "N/A";
        var paymentFee = order.PaymentOption?.Fee.GetValueOrDefault() ?? 0;
        var shipping = db.ShippingOptions.FirstOrDefault(s => s.Id == order.ShippingOptionId);
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
            var product = db.Products.FirstOrDefault(p => p.Id == item.ProductId);
            string productName = product?.Name ?? "Unknown Product";
            decimal lineTotal = item.PricePerUnit * item.Quantity;

            lines.Add($"{item.Quantity} x {productName.PadRight(25)} ({item.PricePerUnit,6:C}) = {lineTotal,8:C}");
        }

        lines.Add($"--------------------------------------------------");
        lines.Add($"TOTAL:         {order.TotalPrice,10:C}");

        GUI.DrawWindow("Order Receipt", 1, 10, lines, 80);
    }
}