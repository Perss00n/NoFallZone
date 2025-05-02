using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Models;
public class Order
{
    public int Id { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? ShippingCost { get; set; }
    public string? PaymentMethod { get; set; }

    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int? ShippingOptionId { get; set; }
    public ShippingOption? ShippingOption { get; set; }

    public ICollection<OrderItem>? OrderItems { get; set; }
}
