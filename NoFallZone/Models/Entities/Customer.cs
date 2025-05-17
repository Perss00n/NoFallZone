using NoFallZone.Models.Enums;

namespace NoFallZone.Models.Entities;
public class Customer
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public int? Age { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public Role Role { get; set; } = Role.User;
    public ICollection<Order>? Orders { get; set; }
}
