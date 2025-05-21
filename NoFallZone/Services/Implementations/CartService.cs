using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Services.Implementations;

public class CartService : ICartService
{
    private readonly NoFallZoneContext _db;

    public CartService(NoFallZoneContext db)
    {
        _db = db;
    }

public void OpenCartMenu()
{
    if (Session.Cart.Count == 0)
    {
        Console.Clear();
        OutputHelper.ShowInfo("Your cart is empty!");
        return;
    }

    bool inCart = true;

    while (inCart)
    {
        Console.Clear();
        var lines = new List<string>();

        for (int i = 0; i < Session.Cart.Count; i++)
        {
            var item = Session.Cart[i];
            decimal total = item.Product.Price * item.Quantity;
            lines.Add($"{i + 1}. {item.Quantity} x {item.Product.Name} ({item.Product.Price:C} each) = {total:C}");
        }

        lines.Add("------------------------");
        lines.Add($"Total: {GetCartTotal():C}");

        GUI.DrawWindow("Your Cart", 1, 1, lines, maxLineWidth: 100);
        GUI.DrawWindow("Options", 1, lines.Count + 3, new List<string>
        {
            "[C] Change quantity",
            "[R] Remove product",
            "[Q] Return to menu"
        });

        var input = Console.ReadKey(true).Key;

        switch (input)
        {
            case ConsoleKey.C:
                int changeIndex = InputHelper.PromptInt("\nEnter item number to change quantity", 1, Session.Cart.Count, $"Enter a valid number from 1 to {Session.Cart.Count}") - 1;
                var item = Session.Cart[changeIndex];
                int newQty = InputHelper.PromptInt($"Enter new quantity for {item.Product.Name}", 1, item.Product.Stock, $"Enter a valid number from 1 to {item.Product.Stock}");

                if (ChangeQuantity(changeIndex, newQty, out var changeMsg))
                    OutputHelper.ShowSuccess(changeMsg);
                else
                    OutputHelper.ShowError(changeMsg);
                break;

            case ConsoleKey.R:
                int removeIndex = InputHelper.PromptInt("\nEnter item number to remove", 1, Session.Cart.Count, $"Enter a valid number from 1 to {Session.Cart.Count}") - 1;
                if (InputHelper.PromptYesNo($"Are you sure you want to delete '{Session.Cart[removeIndex].Product.Name}'?", "Answer only 'Y' for yes or 'N' for no!"))
                {
                    if (RemoveFromCart(removeIndex, out var removeMsg))
                    {
                        OutputHelper.ShowSuccess(removeMsg);
                        if (Session.Cart.Count == 0)
                            inCart = false;
                    }
                }
                else
                    OutputHelper.ShowInfo("Cancelled.");
                break;

            case ConsoleKey.Q:
                inCart = false;
                break;

            default:
                OutputHelper.ShowError("Invalid option.");
                break;
        }

        if (inCart)
        {
            OutputHelper.ShowInfo("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

    public void ShowCartOverview()
    {
        string header = "Your Cart";
        int fromLeft = 73;
        int fromTop = 1;

        if (Session.Cart.Count == 0)
        {
            GUI.DrawWindow(header, fromLeft, fromTop, new List<string>
        {
            "Your cart is empty."
        });
            return;
        }

        var lines = new List<string>();

        for (int i = 0; i < Session.Cart.Count; i++)
        {
            var item = Session.Cart[i];
            lines.Add($"{item.Quantity} x {item.Product.Name} ({item.Product.Price} Each)");
        }

        lines.Add("------------------------");
        lines.Add($"Total: {GetCartTotal():C}");
        lines.Add("Press 'K' to checkout");

        GUI.DrawWindow(header, fromLeft, fromTop, lines, maxLineWidth: 50);
    }

    public void AddDealToCart(ConsoleKey dealKey)
    {
        int dealIndex = dealKey switch
        {
            ConsoleKey.X => 0,
            ConsoleKey.A => 1,
            ConsoleKey.Z => 2,
            _ => -1
        };

        var featuredProducts = _db.Products
            .Where(p => p.IsFeatured)
            .Take(3)
            .ToList();

        if (dealIndex < 0 || dealIndex >= featuredProducts.Count)
        {
            Console.Clear();
            OutputHelper.ShowError("No product available for that deal.");
            return;
        }

        var selectedDeal = featuredProducts[dealIndex];

        if (selectedDeal.Stock <= 0)
        {
            Console.Clear();
            OutputHelper.ShowError("Sorry, the product is out of stock!");
            return;
        }

        if (TryAddToCart(selectedDeal, 1, out string message))
        {
            Console.Clear();
            OutputHelper.ShowSuccess(message);
        }
        else
        {
            Console.Clear();
            OutputHelper.ShowError(message);
        }
    }

    public decimal GetCartTotal()
    {
        return Session.Cart.Sum(item => item.Product.Price * item.Quantity);
    }

    public int GetQuantityInCart(Product product)
    {
        return Session.Cart.FirstOrDefault(i => i.Product.Id == product.Id)?.Quantity ?? 0;
    }

    public int GetAvailableToAdd(Product product)
    {
        return product.Stock - GetQuantityInCart(product);
    }

    public bool TryAddToCart(Product product, int quantity, out string message)
    {
        if (product.Stock <= 0)
        {
            message = "This product is out of stock.";
            return false;
        }

        int available = GetAvailableToAdd(product);
        if (available <= 0)
        {
            message = "You’ve already added the maximum available stock.";
            return false;
        }

        if (quantity > available)
        {
            message = $"Only {available} item(s) left in stock.";
            return false;
        }

        var existing = Session.Cart.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existing != null)
            existing.Quantity += quantity;
        else
            Session.Cart.Add(new CartItem { Product = product, Quantity = quantity });

        message = $"{quantity} x {product.Name} added to cart!";
        return true;
    }

    public bool RemoveFromCart(int index, out string message)
    {
        if (index < 0 || index >= Session.Cart.Count)
        {
            message = "Invalid index.";
            return false;
        }

        var item = Session.Cart[index];
        Session.Cart.RemoveAt(index);
        message = $"{item.Product.Name} removed from cart.";
        return true;
    }

    public bool ChangeQuantity(int index, int newQty, out string message)
    {
        if (index < 0 || index >= Session.Cart.Count)
        {
            message = "Invalid index.";
            return false;
        }

        var item = Session.Cart[index];
        if (newQty > item.Product.Stock)
        {
            message = $"Cannot exceed stock ({item.Product.Stock}).";
            return false;
        }

        item.Quantity = newQty;
        message = $"{item.Product.Name} quantity updated to {newQty}.";
        return true;
    }
}