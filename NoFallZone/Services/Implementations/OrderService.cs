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

    public bool PlaceOrder(int shippingOptionId, string paymentMethod, out string message)
    {
        if (!Session.IsLoggedIn)
        {
            message = "You must be logged in to place an order.";
            return false;
        }

        if (Session.Cart.Count == 0)
        {
            message = "Your cart is empty.";
            return false;
        }

        if (String.IsNullOrEmpty(paymentMethod))
        {
            message = "Invalid payment metod.";
            return false;
        }

        var shipping = db.ShippingOptions.FirstOrDefault(s => s.Id == shippingOptionId);
        if (shipping == null)
        {
            message = "Invalid shipping option.";
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
            PaymentMethod = paymentMethod,
            TotalPrice = Session.GetCartTotal() + shipping.Price,
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

    private void ShowReceipt(Order order)
    {
        Console.Clear();
        var lines = new List<string>
    {
        $"Thank you for your order, {Session.LoggedInUser!.Username}!",
        $"Order Date: {order.OrderDate:G}",
        $"Payment Method: {order.PaymentMethod}",
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