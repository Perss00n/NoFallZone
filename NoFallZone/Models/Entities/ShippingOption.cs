namespace NoFallZone.Models.Entities;
public class ShippingOption
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;
    public decimal Price { get; set; } = decimal.Zero;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
