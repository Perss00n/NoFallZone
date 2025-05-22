namespace NoFallZone.Models.Entities;
public class PaymentOption
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal? Fee { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}