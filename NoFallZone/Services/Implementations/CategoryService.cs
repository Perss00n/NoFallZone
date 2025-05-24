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

    public void ShowAllCategories()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());


        if (!db.Categories.Any())
        {
            GUI.DrawWindow("Categories", 1, 10, new List<string>
        {
            "No categories found in the database."
        });
            return;
        }

        var categories = db.Categories.ToList();

        List<string> outputData = categories.Select(c => $"Id: {c.Id} | Name: {c.Name}").ToList();

        GUI.DrawWindow("Categories", 1, 10, outputData, 60);
    }

    public void AddCategory()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.WriteLine("=== Add a new category ===");

        string categoryName = CategoryValidator.PromptName();

        var newCategory = new Category()
        {
            Name = categoryName
        };

        db.Categories.Add(newCategory);

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The category has been added to the database!");
        else
            OutputHelper.ShowError(errorMsg);
    }
    public void EditCategory()
    {
        if (!RequireAdminAccess()) return;

        var category = CategorySelector.ChooseCategory(db);

        if (category == null) return;

        string? newCategoryName = CategoryValidator.PromptOptionalName(category.Name);
        if (!string.IsNullOrWhiteSpace(newCategoryName))
            category.Name = newCategoryName;

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The category has been updated successfully!");
        else
            OutputHelper.ShowError(errorMsg);
    }
    public void DeleteCategory()
    {
        if (!RequireAdminAccess()) return;

        Console.WriteLine("=== Delete a category ===");

        var category = CategorySelector.ChooseCategory(db);
        if (category == null) return;

        Console.WriteLine($"Are you sure you want to delete the category '{category.Name}'?");

        if (!CategoryValidator.PromptConfirmation())
        {
            OutputHelper.ShowInfo("Cancelled!");
            return;
        }

        db.Categories.Remove(category);

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The category has been deleted successfully!");
        else
            OutputHelper.ShowError(errorMsg);
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
