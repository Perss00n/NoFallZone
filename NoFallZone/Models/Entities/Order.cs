namespace NoFallZone.Models.Entities;
public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; } = 0m;
    public decimal ShippingCost { get; set; } = 0m;
    public int? PaymentOptionId { get; set; }

    public string? PaymentOptionName { get; set; } = "Unknown";
    public PaymentOption? PaymentOption { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int? ShippingOptionId { get; set; }
    public ShippingOption? ShippingOption { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
