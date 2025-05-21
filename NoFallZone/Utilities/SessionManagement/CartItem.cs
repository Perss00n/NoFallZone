using NoFallZone.Models.Entities;

namespace NoFallZone.Utilities.SessionManagement
{
    public class CartItem
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
