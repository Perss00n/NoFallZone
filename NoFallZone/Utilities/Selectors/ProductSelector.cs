using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class ProductSelector
{
    public static async Task<Product?> ChooseProductFromCategoryAsync(Category category, NoFallZoneContext db)
    {
        Console.CursorVisible = true;
        var products = await db.Products
            .Where(p => p.CategoryId == category.Id)
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync();

        if (products.Count == 0)
        {
            Console.Clear();
            OutputHelper.ShowError("No products found in this category!");
            return null;
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        var lines = new List<string>();

        for (int i = 0; i < products.Count; i++)
            lines.Add($"{i + 1}. Name: {products[i].Name} | Supplier: {products[i].Supplier.Name} | Price: {products[i].Price} | Stock: {products[i].Stock} {(products[i].IsFeatured ? "(DEAL)" : "")}");

        GUI.DrawWindow($"Select a product from category '{category.Name}'", 1, 10, lines, 120);

        int productIndex = InputHelper.PromptInt("\nEnter product number", 1, products.Count,
            $"Please select a valid number from 1 to {products.Count}");

        return products[productIndex - 1];
    }

    public static async Task<Product?> ChooseProductFromCategoryAsync(NoFallZoneContext db)
    {
        Console.Clear();
        var category = await CategorySelector.ChooseCategoryAsync(db);
        if (category == null) return null;

        return await ChooseProductFromCategoryAsync(category, db);
    }
}
