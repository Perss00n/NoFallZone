using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Utilities;
using NoFallZone.Models;

namespace NoFallZone.Services;
public class ProductService
{
    private readonly NoFallZoneContext db;

    public ProductService(NoFallZoneContext context)
    {
        db = context;
    }

    public void ShowAllProducts()
    {
        Console.Clear();
        Console.WriteLine("=== Show Products by Category ===\n");

        var categories = db.Categories
            .Include(c => c.Products)
            .ToList();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found in the database. Returning to main menu...");
            Console.ReadKey();
            return;
        }

        for (int i = 0; i < categories.Count; i++)
            Console.WriteLine($"{i + 1}. {categories[i].Name}");

        int selected = InputHelper.PromptInt("\nChoose a category", 1, categories.Count, $"Please enter a valid number from 1 to {categories.Count}! Try again...");
        var category = categories[selected - 1];

        var products = db.Products
            .Where(p => p.CategoryId == category.Id)
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToList();

        var productDetails = products.Select(product => $"[{product.Id}] {product.Name} ({product.Supplier?.Name}) - {product.Price:C} | Stock: {product.Stock}").ToList();

        if (productDetails.Count == 0)
        {
            Console.Clear();
            GUI.DrawWindow($"Products in {category.Name}", 1, 1, new List<string> { "No products available in this category." }, maxLineWidth: 100);
        }
        else
        {
            Console.Clear();
            GUI.DrawWindow($"Products in {category.Name}", 1, 1, productDetails, maxLineWidth: 100);
        }
    }


    public void ShowDeals()
    {

        var deals = db.Products
        .Where(product => product.IsFeatured == true)
        .Take(3)
        .ToList();

        for (int i = 0; i < 3; i++)
        {
            var deal = deals.ElementAtOrDefault(i);
            int fromLeft = i == 0 ? 2 : i == 1 ? 33 : 67;
            int fromTop = 15;

            if (deal != null)
            {
                GUI.DrawWindow($"Deal {i + 1}", fromLeft, fromTop, new List<string>
            {
                deal.Name!,
                deal.Description!,
                $"{deal.Price:C}",
                $"{deal.Stock} pieces left in stock!"
            }, maxLineWidth: 30);
            }
            else
            {
                GUI.DrawWindow($"Deal {i + 1}", fromLeft, fromTop, new List<string>
            {
                "No deal available at this moment"
            });
            }
        }
    }

    public void AddProduct()
    {
        Console.Clear();
        Console.WriteLine("=== Add a new product ===");

        string name = InputHelper.PromptRequiredLimitedString("Product name", 50, "Name is required and must not exceed the maximum length.");

        string description = InputHelper.PromptRequiredLimitedString("Product description", 200, "Description is required and must not exceed the maximum length.");

        decimal price = InputHelper.PromptDecimal("Price", 1m, 10000m, $"Please enter a valid number! Try again...");

        int stock = InputHelper.PromptInt("Quantity in stock", 0, 1000, "Please enter a valid number! Try again...");

        var categories = db.Categories.ToList();
        Console.WriteLine("\nChoose category:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].Name}");
        }
        int categoryIndex = InputHelper.PromptInt("Enter the number of the category", 1, categories.Count, $"Please enter a valid number from 1 to {categories.Count}! Try again...");
        int categoryId = categories[categoryIndex - 1].Id;

        var suppliers = db.Suppliers.ToList();
        Console.WriteLine("\nChoose supplier:");
        for (int i = 0; i < suppliers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {suppliers[i].Name}");
        }
        int supplierIndex = InputHelper.PromptInt("Enter the number of the supplier", 1, suppliers.Count, $"Please enter a valid number from 1 to {suppliers.Count}! Try again...");
        int supplierId = suppliers[supplierIndex - 1].Id;

        bool isFeatured = InputHelper.PromptYesNo("Should the product be displayed as an offer?", "Please enter only 'Y' for Yes and 'N' for No! Try again...");

        var product = new Product
        {
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
            CategoryId = categoryId,
            SupplierId = supplierId,
            IsFeatured = isFeatured
        };

        db.Products.Add(product);
        db.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nThe product has been added to the database!");
        Console.ResetColor();
    }


    public void EditProduct()
    {
        Console.Clear();
        Console.WriteLine("=== Edit Product ===");

        var categories = db.Categories
            .Include(c => c.Products)
            .ToList();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found in the database. Returning to main menu...");
            return;
        }

            Console.WriteLine("\nSelect a category:");
        for (int i = 0; i < categories.Count; i++)
            Console.WriteLine($"{i + 1}. {categories[i].Name}");

        int categoryIndex = InputHelper.PromptInt("Enter number", 1, categories.Count, $"Please select a valid category from 1 to {categories.Count}! Try again...");
        var selectedCategory = categories[categoryIndex - 1];

        var products = selectedCategory.Products!.ToList();

        if (products.Count == 0)
        {
            Console.WriteLine("No products found in this category. Returning to main menu...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("\nSelect a product to edit:");
        for (int i = 0; i < products.Count; i++)
            Console.WriteLine($"{i + 1}. {products[i].Name}");

        int productIndex = InputHelper.PromptInt("Enter number", 1, products.Count, $"Please select a valid product from 1 to {products.Count}! Try again...");
        var product = products[productIndex - 1];

        Console.Clear();
        Console.WriteLine($"=== Editing '{product.Name}' ===");

        string newName = InputHelper.PromptOptionalLimitedString($"Name [{product.Name}]", 50, "The name is too long! Try again...");
        if (!string.IsNullOrWhiteSpace(newName)) product.Name = newName;

        string newDesc = InputHelper.PromptOptionalLimitedString($"Description [{product.Description}]", 200, "The description is too long! Try again...");
        if (!string.IsNullOrWhiteSpace(newDesc)) product.Description = newDesc;

        var newPrice = InputHelper.PromptOptionalDecimal($"Price [{product.Price}]", 1m, 10000m, "Please select a valid number! Try again...");
        if (newPrice.HasValue) product.Price = newPrice.Value;

        var newStock = InputHelper.PromptOptionalInt($"Stock [{product.Stock}]", 0, 1000, "Please select a valid number! Try again...");
        if (newStock.HasValue) product.Stock = newStock.Value;

        product.IsFeatured = InputHelper.PromptYesNo($"Should the product be displayed as an offer? Currently it {(product.IsFeatured == true ? "IS set to an offer" : "is NOT set to an offer")}", "Please enter only 'Y' for Yes and 'N' for No! Try again...");

        db.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nProduct updated successfully!");
        Console.ResetColor();
    }


    public void DeleteProduct()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Product ===");


            List<Category> categories = db.Categories
                .Include(c => c.Products)
                .ToList();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found in the database. Returning to main menu...");
            return;
        }

            Console.WriteLine("\nSelect a category:");
            for (int i = 0; i < categories.Count; i++)
                Console.WriteLine($"{i + 1}. {categories[i].Name}");

            int catIndex = InputHelper.PromptInt("Enter number", 1, categories.Count, $"Please select a valid category from 1 to {categories.Count}");
            var selectedCategory = categories[catIndex - 1];

            var products = selectedCategory.Products!.ToList();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found in this category. Returning to main menu...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nSelect a product to delete:");
            for (int i = 0; i < products.Count; i++)
                Console.WriteLine($"{i + 1}. {products[i].Name}");

            int productIndex = InputHelper.PromptInt("Enter number", 1, products.Count, $"Please select a valid product from 1 to {products.Count}");
            var product = products[productIndex - 1];

            Console.Clear();
            Console.WriteLine($"Are you sure you want to delete '{product.Name}'?");
            bool confirm = InputHelper.PromptYesNo("Confirm deletion", "Please enter only 'Y' for Yes and 'N' for No! Try again...");

            if (confirm)
            {
                db.Products.Remove(product);
                db.SaveChanges();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Product deleted succesfully!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Deletion cancelled!");
                Console.ResetColor();
            }
        }
    }
