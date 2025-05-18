using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Validators;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Session;

namespace NoFallZone.Services.Implementations;
public class ProductService : IProductService
{
    private readonly NoFallZoneContext db;

    public ProductService(NoFallZoneContext context)
    {
        db = context;
    }

    public void ShowProducts()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        var category = CategorySelector.ChooseCategory(db);
        if (category == null) return;

        var products = db.Products
            .Where(p => p.CategoryId == category.Id)
            .Include(p => p.Supplier)
            .ToList();

        Console.Clear();

        if (products.Count == 0)
        {
            GUI.DrawWindow($"Products in {category.Name}", 1, 2,
                new List<string> { "No products available in this category." },
                maxLineWidth: 70);
            return;
        }

        int fromTop = 2;
        foreach (var p in products)
        {
            GUI.DrawWindow($"Product: {p.Name}", 1, fromTop, new List<string>
        {
            $"ID:        {p.Id}",
            $"Name:      {p.Name}",
            $"Price:     {p.Price:C}",
            $"Stock:     {p.Stock}",
            $"Category:  {category.Name}",
            $"Supplier:  {p.Supplier!.Name}",
            $"Featured:  {(p.IsFeatured == true ? "Yes" : "No")}"
        }, maxLineWidth: 70);

            fromTop += 10;
        }
    }

    public void SearchProducts()
    {
        Console.Clear();
        Console.Write("Enter a keyword to search for (or 'Q' to cancel): ");
        string keyword = Console.ReadLine()!.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(keyword) || keyword == "q")
        {
            OutputHelper.ShowInfo("Search cancelled!");
            return;
        }

        var results = db.Products
            .Where(p =>
                p.Name!.ToLower().Contains(keyword) ||
                p.Description!.ToLower().Contains(keyword) ||
                p.Supplier!.Name!.ToLower().Contains(keyword))
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToList();

        Console.Clear();

        if (results.Count == 0)
        {
            OutputHelper.ShowError("No products matched your search.");
            return;
        }

        var outputData = results
            .Select((p, i) => $"{i + 1}. {p.Name} || {p.Category?.Name} || {p.Price:C}")
            .ToList();

        GUI.DrawWindow("Matching products", 0, 0, outputData, maxLineWidth: 60);

        int index = InputHelper.PromptInt("\nEnter the number of the product you want to view the details of", 1, results.Count,
            $"Please enter a number from 1 to {results.Count}");

        var selectedProduct = results[index - 1];
        ShowProductDetails(selectedProduct);
    }

    public void ShowProductDetails(Product product)
    {
        Console.Clear();

        var outputData = new List<string>
    {
        $"Name:        {product.Name}",
        $"Description: {product.Description}",
        $"Price:       {product.Price:C}",
        $"Stock:       {product.Stock}",
        $"Category:    {product.Category?.Name}",
        $"Supplier:    {product.Supplier?.Name}"
    };

        GUI.DrawWindow("Product Details", 15, 1, outputData, maxLineWidth: 80);

        bool waitingForValidInput = true;

        while (waitingForValidInput)
        {
            Console.WriteLine();
            Console.WriteLine("1. Add product to cart");
            Console.WriteLine("2. Return to main menu");

            var input = Console.ReadKey(true).Key;

            switch (input)
            {
                case ConsoleKey.D1:
                    OutputHelper.ShowSuccess("Product added to cart!");
                    waitingForValidInput = false;
                    break;

                case ConsoleKey.D2:
                    OutputHelper.ShowInfo("Returning to main menu...");
                    waitingForValidInput = false;
                    break;

                default:
                    OutputHelper.ShowError("Invalid choice! Please try again.");
                    break;
            }
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
            int fromLeft = i == 0 ? 0 : i == 1 ? 40 : 78;
            int fromTop = 15;
            string dealBuyKeyMessage = i == 0 ? "Press X to buy" : i == 1 ? "Press A to buy" : "Press Z to buy";

            if (deal != null)
            {
                GUI.DrawWindow($"Deal {i + 1}", fromLeft, fromTop, new List<string>
            {
                deal.Name!,
                deal.Description!,
                $"{deal.Price:C}",
                $"{deal.Stock} pieces left in stock!",
                "",
                dealBuyKeyMessage
            }, maxLineWidth: 33);
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
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== Add a new product ===");

        var category = CategorySelector.ChooseCategory(db);
        if (category == null) return;
        int categoryId = category.Id;

        var supplier = SupplierSelector.ChooseSupplier(db);
        if (supplier == null) return;
        int supplierId = supplier.Id;

        string name = ProductValidator.PromptName();
        string description = ProductValidator.PromptDescription();
        decimal price = ProductValidator.PromptPrice();
        int stock = ProductValidator.PromptStock();


        Console.WriteLine("Should the product be displayed as an offer?");
        bool isFeatured = ProductValidator.PromptConfirmation();

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

        OutputHelper.ShowSuccess("The product has been added to the database!");
    }


    public void EditProduct()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== Edit Product ===");

        var product = ProductSelector.ChooseProductFromCategory(db);
        if (product == null) return;

        Console.Clear();
        Console.WriteLine($"=== Editing '{product.Name}' ===");

        string? newName = ProductValidator.PromptOptionalName(product.Name!);
        if (!string.IsNullOrWhiteSpace(newName))
            product.Name = newName;

        string? newDesc = ProductValidator.PromptOptionalDescription(product.Description!);
        if (!string.IsNullOrWhiteSpace(newDesc))
            product.Description = newDesc;

        decimal? newPrice = ProductValidator.PromptOptionalPrice(product.Price ?? 0);
        if (newPrice.HasValue)
            product.Price = newPrice;

        int? newStock = ProductValidator.PromptOptionalStock(product.Stock ?? 0);
        if (newStock.HasValue)
            product.Stock = newStock;

        Console.WriteLine($"\nShould the product be displayed as an offer?");
        Console.WriteLine($"Currently it {(product.IsFeatured == true ? "IS set to an offer" : "is NOT set to an offer")}");
        product.IsFeatured = ProductValidator.PromptConfirmation();

        db.SaveChanges();

        OutputHelper.ShowSuccess("Product updated successfully!");
    }

    public void DeleteProduct()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== Delete Product ===");

        var product = ProductSelector.ChooseProductFromCategory(db);
        if (product == null) return;

        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete '{product.Name}'?");

        bool confirm = ProductValidator.PromptConfirmation();

        if (confirm)
        {
            db.Products.Remove(product);
            db.SaveChanges();

            OutputHelper.ShowSuccess("Product deleted successfully!");
        }
        else
        {
            OutputHelper.ShowError("Deletion cancelled! Returning to main menu...");
        }
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
