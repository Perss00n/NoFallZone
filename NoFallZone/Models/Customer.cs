using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Models;
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
    public string Role { get; set; } = "user";



    public ICollection<Order>? Orders { get; set; }
}
