using NoFallZone.Models.Enums;

namespace NoFallZone.Models.Entities;
public class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public int? Age { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.User;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}

