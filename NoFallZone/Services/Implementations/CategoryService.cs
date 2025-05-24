using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Utilities.Validators;

namespace NoFallZone.Services.Implementations;
public class CategoryService : ICategoryService
{

    private readonly NoFallZoneContext db;

    public CategoryService(NoFallZoneContext context)
    {
        db = context;
    }

    public async Task ShowAllCategoriesAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var categories = await db.Categories.ToListAsync();

        if (categories.Count == 0)
        {
            GUI.DrawWindow("Categories", 1, 10, new List<string>
        {
            "No categories found in the database."
        });
            return;
        }

        List<string> outputData = categories
            .Select(c => $"Id: {c.Id} | Name: {c.Name}")
            .ToList();

        GUI.DrawWindow("Categories", 1, 10, outputData, 60);
    }

    public async Task AddCategoryAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("=== Add a new category ===");

        string categoryName = CategoryValidator.PromptName();

        var newCategory = new Category()
        {
            Name = categoryName
        };

       await db.Categories.AddAsync(newCategory);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The category has been added to the database!");
    }
    public async Task EditCategoryAsync()
    {
        if (!RequireAdminAccess()) return;

        var category = await CategorySelector.ChooseCategoryAsync(db);
        if (category == null) return;

        string? newCategoryName = CategoryValidator.PromptOptionalName(category.Name);
        if (!string.IsNullOrWhiteSpace(newCategoryName))
            category.Name = newCategoryName;

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The category has been updated successfully!");
    }
    public async Task DeleteCategoryAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.WriteLine("=== Delete a category ===");

        var category = await CategorySelector.ChooseCategoryAsync(db);
        if (category == null) return;

        OutputHelper.ShowInfo($"Are you sure you want to delete the category '{category.Name}'?");

        if (!CategoryValidator.PromptConfirmation())
        {
            OutputHelper.ShowInfo("Cancelled!");
            return;
        }

        db.Categories.Remove(category);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The category has been deleted successfully!");
    }
    private bool RequireAdminAccess()
    {
        if (!Session.IsAdmin)
        {
            OutputHelper.ShowError("Access Denied!");
            return false;
        }
        return true;
    }
}
