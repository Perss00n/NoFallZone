using NoFallZone.Models.Enums;

namespace NoFallZone.Models.Entities;
public class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; } = string.Empty;

    public string Address { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;

    public int? Age { get; set; } = 0!;

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public Role Role { get; set; } = Role.User;

    public ICollection<Order>? Orders { get; set; }
}

