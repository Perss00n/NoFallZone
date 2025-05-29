namespace NoFallZone.Models.Entities;
public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; } = decimal.Zero;
    public int Stock { get; set; } = 0;
    public bool IsFeatured { get; set; } = false;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
