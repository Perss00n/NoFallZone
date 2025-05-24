using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class CategorySelector
{
    public static async Task<Category?> ChooseCategoryAsync(NoFallZoneContext db)
    {
        Console.CursorVisible = true;

        var categories = await db.Categories
            .Include(c => c.Products)
            .ToListAsync();

        if (categories.Count == 0)
        {
            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            OutputHelper.ShowError("No categories found!");
            return null;
        }
        
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var lines = new List<string>();
        for (int i = 0; i < categories.Count; i++)
            lines.Add($"{i + 1}. {categories[i].Name}");

        GUI.DrawWindow("Choose a category", 1, 10, lines, 100);

        int index = InputHelper.PromptInt("\nEnter category number", 1, categories.Count,
            $"Please enter a number from 1 to {categories.Count}");

        return categories[index - 1];
    }

}