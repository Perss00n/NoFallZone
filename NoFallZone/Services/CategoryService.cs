using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models;
using NoFallZone.Utilities;

namespace NoFallZone.Services;
public class CategoryService : ICategoryService
{

    private readonly NoFallZoneContext db;

    public CategoryService(NoFallZoneContext context)
    {
        db = context;
    }

    public void ShowAllCategories()
    {
        Console.Clear();
        Console.WriteLine("=== All Categories ===\n");

        var categories = db.Categories.ToList();

        if (categories.Count == 0)
        {
            GUI.DrawWindow("Categories", 1, 2, new List<string>
        {
            "No categories found in the database."
        });
            return;
        }

        List<string> outputData = categories.Select(c => $"Id: {c.Id} | Name: {c.Name}").ToList();

        GUI.DrawWindow("Categories", 1, 2, outputData, maxLineWidth: 60);
    }

    public void AddCategory()
    {

        Console.Clear();
        Console.WriteLine("=== Add a new category ===");

        string categoryName = CategoryValidator.PromptName();

        var newCategory = new Category()
        {
            Name = categoryName
        };

        db.Categories.Add(newCategory);
        db.SaveChanges();

        OutputHelper.ShowSuccess("The Category has been added to the database!");
    }
    public void EditCategory()
    {
        var category = CategorySelector.ChooseCategory(db);

        if (category == null) return;

        string? newCategoryName = CategoryValidator.PromptOptionalName(category.Name!);
        if (!string.IsNullOrWhiteSpace(newCategoryName))
            category.Name = newCategoryName;

        db.SaveChanges();

        OutputHelper.ShowSuccess("Category updated successfully!");
    }
    public void DeleteCategory()
    {

        Console.Clear();
        Console.WriteLine("=== Delete a category ===");

        var category = CategorySelector.ChooseCategory(db);
        if (category == null) return;

        Console.Clear();

        Console.WriteLine($"Are you sure you want to delete the category '{category.Name}'?");

        bool confirm = CategoryValidator.PromptConfirmation();

        if (confirm)
        {
            db.Categories.Remove(category);
            db.SaveChanges();

            OutputHelper.ShowSuccess("Category deleted successfully!");
        }
        else
        {
            OutputHelper.ShowError("Deletion cancelled! Returning to main menu...");
        }

    }
}
