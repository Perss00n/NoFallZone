using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
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

    public void ShowOrderPreview(ShippingOption shipping, PaymentOption payment)
    {
        Console.Clear();

        var lines = new List<string>
    {
        $"Customer:  {Session.LoggedInUser!.Username}",
        $"Shipping:  {shipping.Name} ({shipping.Price:C})",
        $"Payment:   {payment.Name} (Fee: {payment.Fee:C})",
        $"------------------------------"
    };

        foreach (var item in Session.Cart)
        {
            var product = item.Product;
            decimal lineTotal = product.Price * item.Quantity;

            lines.Add($"{item.Quantity} x {product.Name} ({product.Price:C} each) = {lineTotal:C}");
        }

        decimal subtotal = Session.GetCartTotal();
        decimal total = subtotal + shipping.Price + (payment.Fee ?? 0);

        lines.Add($"------------------------------");
        lines.Add($"Subtotal: {subtotal:C}");
        lines.Add($"Shipping: {shipping.Price:C}");
        lines.Add($"Payment Fee: {(payment.Fee ?? 0):C}");
        lines.Add($"------------------------------");
        lines.Add($"Total: {total:C}");

        GUI.DrawWindow("Order Summary (Preview)", 1, 1, lines, maxLineWidth: 80);
    }

    private void ShowReceipt(Order order)
    {
        Console.Clear();
        var lines = new List<string>
    {
        $"Thank you for your order, {Session.LoggedInUser!.Username}!",
        $"Order Date: {order.OrderDate:G}",
        $"Payment: {order.PaymentOption?.Name} (Fee: {order.PaymentOption?.Fee:C})",
        $"Shipping: {order.ShippingCost:C} via {db.ShippingOptions.First(s => s.Id == order.ShippingOptionId).Name}",
        $"------------------------------"
    };

        foreach (var item in order.OrderItems)
        {
            var product = db.Products.First(p => p.Id == item.ProductId);
            decimal lineTotal = item.PricePerUnit * item.Quantity;

            lines.Add($"{item.Quantity} x {product.Name} ({item.PricePerUnit:C} each) = {lineTotal:C}");
        }

        lines.Add($"------------------------------");
        lines.Add($"Total: {order.TotalPrice:C}");

        GUI.DrawWindow("Order Receipt", 1, 1, lines, maxLineWidth: 80);
    }
}