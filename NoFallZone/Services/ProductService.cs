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
        var products = db.Products
                         .Include(product => product.Category)
                         .Include(product => product.Supplier)
                         .ToList();

        var productDetails = products.Select(product =>
            $"[{product.Id}] {product.Name} ({product.Category?.Name}) - {product.Price:C} | Stock: {product.Stock}"
        ).ToList();

        GUI.DrawWindow("Products", 20, 1, productDetails, maxLineWidth: 100);
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

        string name = InputHelper.Prompt("Product name", productName => !string.IsNullOrWhiteSpace(productName), "Product name can't be empty! Please try again...");

        string description = InputHelper.Prompt("Product description", productDescription => !string.IsNullOrWhiteSpace(productDescription), "Product description can't be empty! Please try again...");

        decimal price = InputHelper.PromptDecimal("Price", 1m, 10000m, $"Please enter a valid number from 1 to 10 000! Try again...");

        int stock = InputHelper.PromptInt("Quantity in stock", 0, 1000, "Please enter a valid number from 1 to 1000! Try again...");

        // Hämta alla kategorier från databasen
        var categories = db.Categories.ToList();
        Console.WriteLine("\nChoose category:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categories[i].Name}");
        }
        int categoryIndex = InputHelper.PromptInt("Enter the number of the category", 1, categories.Count, $"Please enter a valid number from 1 to {categories.Count}! Try again...");
        int categoryId = categories[categoryIndex - 1].Id;

        // Hämta alla leverantörer från databasen
        var suppliers = db.Suppliers.ToList();
        Console.WriteLine("\nChoose supplier:");
        for (int i = 0; i < suppliers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {suppliers[i].Name}");
        }
        int supplierIndex = InputHelper.PromptInt("Enter the number of the supplier", 1, suppliers.Count, $"Please enter a valid number from 1 to {suppliers.Count}! Try again...");
        int supplierId = suppliers[supplierIndex - 1].Id;

        bool isFeatured = InputHelper.PromptYesNo("Should the product be displayed as an offer?", "Please enter only 'Y' for Yes and 'N' for No! Try again...");

        // Skapa produkten
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



}
