namespace NoFallZone.Models.Entities;
public class Supplier
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
