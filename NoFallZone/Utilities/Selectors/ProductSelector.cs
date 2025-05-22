using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class ProductSelector
{
    public static Product? ChooseProductFromCategory(Category category, NoFallZoneContext db)
    {
        Console.Clear();
        Console.CursorVisible = true;
        var products = db.Products
            .Where(p => p.CategoryId == category.Id)
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToList();

        if (products.Count == 0)
        {
            OutputHelper.ShowError("No products found in this category!");
            return null;
        }

        var lines = new List<string>();

        for (int i = 0; i < products.Count; i++)
            lines.Add($"{i + 1}. {products[i].Name} || Price: {products[i].Price}");

        GUI.DrawWindow($"Select a product from category '{category.Name}'", 1, 1, lines, maxLineWidth: 100);

        int productIndex = InputHelper.PromptInt("\nEnter product number", 1, products.Count,
            $"Please select a valid number from 1 to {products.Count}");

        return products[productIndex - 1];
    }

    public static Product? ChooseProductFromCategory(NoFallZoneContext db)
    {
        Console.Clear();
        var category = CategorySelector.ChooseCategory(db);
        if (category == null) return null;

        return ChooseProductFromCategory(category, db);
    }




}
