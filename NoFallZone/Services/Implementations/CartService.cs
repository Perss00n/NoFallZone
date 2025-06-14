﻿using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Services.Implementations;

public class CartService : ICartService
{
    private readonly NoFallZoneContext _db;
    private readonly IOrderService _orderService;

    public CartService(NoFallZoneContext db, IOrderService orderService)
    {
        _db = db;
        _orderService = orderService;
    }

    public async Task OpenCartMenuAsync()
    {
        if (Session.Cart.Count == 0)
        {
            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            OutputHelper.ShowInfo("Your cart is empty!");
            return;
        }

        bool inCart = true;

        while (inCart)
        {
            PrintCartWindow();

            var input = Console.ReadKey(true).Key;

            switch (input)
            {
                case ConsoleKey.D1:
                    PromptChangeQuantity();
                    break;
                case ConsoleKey.D2:
                    inCart = PromptRemoveProduct();
                    break;
                case ConsoleKey.D3:
                    inCart = await HandleCheckoutAsync();
                    break;
                case ConsoleKey.D4:
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

    public void ShowStartPageCartOverview()
    {
        string header = "Your Cart";
        int fromLeft = Session.IsUser ? 45 : 65;
        int fromTop = 9;

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
            lines.Add($"{item.Quantity} x {OutputHelper.Truncate(item.Product.Name, 27)} ({item.Product.Price:C} Each)");
        }

        lines.Add("------------------------");
        lines.Add($"Total: {GetCartTotal():C}");
        lines.Add("Press 'K' to checkout");

        GUI.DrawWindow(header, fromLeft, fromTop, lines);
    }

    public async Task AddDealToCartAsync(ConsoleKey dealKey)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        int dealIndex = dealKey switch
        {
            ConsoleKey.X => 0,
            ConsoleKey.A => 1,
            ConsoleKey.Z => 2,
            _ => -1
        };

        var featuredProducts = await _db.Products
            .Where(p => p.IsFeatured)
            .Take(3)
            .ToListAsync();

        if (dealIndex < 0 || dealIndex >= featuredProducts.Count)
        {
            OutputHelper.ShowError("No product available for that deal.");
            return;
        }

        var selectedDeal = featuredProducts[dealIndex];

        if (selectedDeal.Stock <= 0)
        {
            OutputHelper.ShowError("Sorry, the product is out of stock!");
            return;
        }

        if (TryAddToCart(selectedDeal, 1, out string message))
            OutputHelper.ShowSuccess(message);
        else
            OutputHelper.ShowError(message);
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

    private decimal GetCartTotal()
    {
        return Session.Cart.Sum(item => item.Product.Price * item.Quantity);
    }

    private int GetQuantityInCart(Product product)
    {
        return Session.Cart.FirstOrDefault(i => i.Product.Id == product.Id)?.Quantity ?? 0;
    }

    private bool RemoveFromCart(int index, out string message)
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

    private bool ChangeQuantity(int index, int newQty, out string message)
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

    private void PrintCartWindow()
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        var lines = new List<string>();

        for (int i = 0; i < Session.Cart.Count; i++)
        {
            var item = Session.Cart[i];
            decimal total = item.Product.Price * item.Quantity;
            lines.Add($"{i + 1}. {item.Quantity} x {OutputHelper.Truncate(item.Product.Name, 58)} ({item.Product.Price:C} each) = {total:C}");
        }

        lines.Add("------------------------");
        lines.Add($"Total: {GetCartTotal():C}");

        GUI.DrawWindow("Your Cart", 1, 10, lines, 100);
        GUI.DrawWindow("Options", 1, lines.Count + 12, new List<string>
    {
        "1. Change quantity",
        "2. Remove product",
        "3. Checkout",
        "4. Return to menu"
    });
    }

    private void PromptChangeQuantity()
    {
        int index = InputHelper.PromptInt("\nEnter item number to change quantity", 1, Session.Cart.Count, $"Enter a valid number from 1 to {Session.Cart.Count}") - 1;
        var item = Session.Cart[index];

        int newQty = InputHelper.PromptInt($"Enter new quantity for {item.Product.Name}", 1, item.Product.Stock, $"Enter a valid number from 1 to {item.Product.Stock}");

        if (ChangeQuantity(index, newQty, out var msg))
            OutputHelper.ShowSuccess(msg);
        else
            OutputHelper.ShowError(msg);
    }

    private bool PromptRemoveProduct()
    {
        int index = InputHelper.PromptInt("\nEnter item number to remove", 1, Session.Cart.Count, $"Enter a valid number from 1 to {Session.Cart.Count}") - 1;

        if (InputHelper.PromptYesNo($"Are you sure you want to delete '{Session.Cart[index].Product.Name}'?", "Answer only 'Y' for yes or 'N' for no!"))
        {
            if (RemoveFromCart(index, out var msg))
            {
                OutputHelper.ShowSuccess(msg);
                return Session.Cart.Count > 0;
            }
        }
        else
        {
            OutputHelper.ShowInfo("Cancelled.");
        }

        return true;
    }

    private async Task<bool> HandleCheckoutAsync()
    {
        Console.Clear();

        var selectedShipping = await ShippingSelector.ChooseShippingAsync(_db);
        if (selectedShipping == null) return true;

        var selectedPayment = await PaymentSelector.ChoosePaymentOptionAsync(_db);
        if (selectedPayment == null) return true;

        _orderService.ShowOrderPreview(selectedShipping, selectedPayment);

        if (InputHelper.PromptYesNo("\nAre you sure you want to place this order?", "Enter Y for Yes or N for No"))
        {
            bool success = await _orderService.PlaceOrderAsync(selectedShipping.Id, selectedPayment.Id);

            if (success)
            {
                OutputHelper.ShowSuccess("Order placed successfully!");
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            OutputHelper.ShowInfo("Order cancelled!");
        }

        return true;
    }

}