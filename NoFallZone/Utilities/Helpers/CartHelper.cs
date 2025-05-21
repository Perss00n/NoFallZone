using NoFallZone.Menu;
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


    public static bool TryAddToCart(Product product, int quantityToAdd, out string message)
    {
        int available = GetAvailableQuantityToAdd(product);

        if (quantityToAdd > available)
        {
            message = $"Cannot add {quantityToAdd} of {product.Name}! You've already added the maximum available stock.";
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

        message = $"{quantityToAdd} x {product.Name} added to cart!";
        return true;
    }


    public static void PrintCartItems()
    {
        Console.Clear();
        var lines = new List<string>();

        for (int i = 0; i < Session.Cart.Count; i++)
        {
            var item = Session.Cart[i];
            decimal itemTotal = item.Product.Price * item.Quantity;

            lines.Add($"{i + 1}. {item.Quantity} x {item.Product.Name} ({item.Product.Price} Each) = {itemTotal:C}");
        }

        lines.Add("------------------------");
        lines.Add($"Total: {Session.GetCartTotal():C}");

        GUI.DrawWindow("Your Cart", 1, 1, lines, maxLineWidth: 100);

        GUI.DrawWindow("Options", 1, lines.Count + 3, new List<string>
                {
                    "[C] Change quantity",
                    "[R] Remove product",
                    "[Q] Return to menu"
                });
    }

    public static void ChangeCartQuantity()
    {
        int indexToChange = InputHelper.PromptInt("\nEnter item number to change quantity", 1, Session.Cart.Count,
            $"Choose a number between 1 and {Session.Cart.Count}") - 1;

        var itemToChange = Session.Cart[indexToChange];
        int newQty = InputHelper.PromptInt($"\nEnter new quantity for {itemToChange.Product.Name}", 1, itemToChange.Product.Stock,
            $"Enter a number between 1 and {itemToChange.Product.Stock}");

        itemToChange.Quantity = newQty;
        OutputHelper.ShowSuccess($"Updated quantity of {itemToChange.Product.Name} to {newQty}");
    }


    public static bool DeleteItemFromCart(out string message)
    {
        int itemIndex = InputHelper.PromptInt("\nEnter item number to remove", 1, Session.Cart.Count,
            $"Choose a number between 1 and {Session.Cart.Count}") - 1;

        var item = Session.Cart[itemIndex];
        bool confirmed = InputHelper.PromptYesNo(
            $"Are you sure you want to remove {item.Product.Name} from your cart?",
            "Please answer 'Y' for Yes or 'N' for No!");

        if (!confirmed)
        {
            message = "The action was cancelled!";
            return false;
        }

        Session.Cart.RemoveAt(itemIndex);
        message = $"{item.Product.Name} removed from cart!";
        return true;
    }

}