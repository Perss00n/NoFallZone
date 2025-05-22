using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class CategorySelector
{
    public static Category? ChooseCategory(NoFallZoneContext db)
    {
        Console.Clear();
        Console.CursorVisible = true;
        var categories = db.Categories
            .Include(c => c.Products)
            .ToList();

        if (categories.Count == 0)
        {
            OutputHelper.ShowError("No categories found! Returning to main menu...");
            return null;
        }

        var lines = new List<string>();
        for (int i = 0; i < categories.Count; i++)
            lines.Add($"{i + 1}. {categories[i].Name}");

        GUI.DrawWindow("Choose a category", 1, 1, lines, maxLineWidth: 100);

        int index = InputHelper.PromptInt("\nEnter category number", 1, categories.Count,
            $"Please enter a number from 1 to {categories.Count}");

        return categories[index - 1];
    }
}