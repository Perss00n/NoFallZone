using NoFallZone.Models.Entities;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Utilities.Helpers;
public class CartHelper
{
    public static int GetQuantityInCart(Product product)
    {
        var item = Session.Cart.FirstOrDefault(c => c.Product.Id == product.Id);
        return item?.Quantity ?? 0;
    }


    public static int GetAvailableQuantityToAdd(Product product)
    {
        int inCart = GetQuantityInCart(product);
        return product.Stock - inCart;
    }


    public static bool TryAddToCart(Product product, int quantityToAdd)
    {
        int available = GetAvailableQuantityToAdd(product);

        if (quantityToAdd > available)
        {
            return false;
        }

        var existingItem = Session.Cart.FirstOrDefault(c => c.Product.Id == product.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += quantityToAdd;
        }
        else
        {
            Session.Cart.Add(new CartItem { Product = product, Quantity = quantityToAdd });
        }

        return true;
    }
}