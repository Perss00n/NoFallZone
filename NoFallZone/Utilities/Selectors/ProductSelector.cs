using NoFallZone.Data;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class ProductSelector
{
    public static Product? ChooseProductFromCategory(NoFallZoneContext db)
    {
        var category = CategorySelector.ChooseCategory(db);
        if (category == null) return null;

        var products = db.Products
            .Where(p => p.CategoryId == category.Id)
            .ToList();

        if (products.Count == 0)
        {
            OutputHelper.ShowError("No products found in this category! Returning to main menu...");
            return null;
        }

        Console.WriteLine($"\nSelect a product from category '{category.Name}':");
        for (int i = 0; i < products.Count; i++)
            Console.WriteLine($"{i + 1}. {products[i].Name}");

        int productIndex = InputHelper.PromptInt("Enter number", 1, products.Count,
            $"Please select a valid product from 1 to {products.Count}");

        return products[productIndex - 1];
    }
}
