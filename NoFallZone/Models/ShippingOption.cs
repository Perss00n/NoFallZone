using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Models;
public class ShippingOption
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }

    public ICollection<Order>? Orders { get; set; }
}
