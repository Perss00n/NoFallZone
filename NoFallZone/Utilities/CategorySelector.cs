using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Models;

namespace NoFallZone.Utilities;
public static class CategorySelector
{
    public static Category? ChooseCategory(NoFallZoneContext db)
    {
        var categories = db.Categories
            .Include(c => c.Products)
            .ToList();

        if (categories.Count == 0)
        {
            OutputHelper.ShowError("No categories found! Returning to main menu...");
            return null;
        }

        Console.WriteLine("\nChoose a category:");
        for (int i = 0; i < categories.Count; i++)
            Console.WriteLine($"{i + 1}. {categories[i].Name}");

        int index = InputHelper.PromptInt("Enter category number", 1, categories.Count,
            $"Please enter a number from 1 to {categories.Count}");

        return categories[index - 1];
    }
}