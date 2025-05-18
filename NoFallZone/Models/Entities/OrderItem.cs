namespace NoFallZone.Models.Entities;
public class OrderItem
{
    public int Id { get; set; }

    public int Quantity { get; set; } = 1;
    public decimal PricePerUnit { get; set; } = 0m;

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}