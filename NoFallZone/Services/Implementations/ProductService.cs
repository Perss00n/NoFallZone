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
public class ProductService : IProductService
{
    private readonly NoFallZoneContext db;
    private readonly ICartService _cartService;

    public ProductService(NoFallZoneContext context, ICartService cartService)
    {
        db = context;
        _cartService = cartService;
    }

    public async Task ShowShopProductsAsync()
    {
        Console.Clear();
        Console.CursorVisible = true;
        var category = await CategorySelector.ChooseCategoryAsync(db);
        if (category == null) return;

        var product = await ProductSelector.ChooseProductFromCategoryAsync(category, db);
        if (product == null) return;

        ShowProductDetails(product);
    }

    public async Task SearchProductsAsync()
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.CursorVisible = true;
        Console.Write("\nEnter a keyword to search for (Leave empty to cancel): ");
        string keyword = Console.ReadLine()!.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(keyword))
        {
            OutputHelper.ShowInfo("Search cancelled!");
            return;
        }

        await LogHelper.LogAsync(db, "NewProductSearch", $"{keyword}");
        var results = await db.Products
            .Where(p =>
                p.Name.ToLower().Contains(keyword) ||
                p.Description.ToLower().Contains(keyword) ||
                (p.Supplier != null && p.Supplier.Name.ToLower().Contains(keyword)))
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToListAsync();

        Console.Clear();

        if (results.Count == 0)
        {
            OutputHelper.ShowError("No products matched your search.");
            return;
        }

        var outputData = results.Select((p, i) =>
        {
            string index = (i + 1).ToString();
            string name = p.Name;
            string category = p.Category.Name;
            string supplier = p.Supplier.Name;
            string price = p.Price.ToString("C");
            int stock = p.Stock;
            string dealTag = p.IsFeatured ? "(DEAL)" : "";

            return $"{index}. Name: {name} | Category: {category} | Supplier: {supplier} | Price: {price} | Stock: {stock} {dealTag}";
        }).ToList();

        Console.WriteLine(DisplayHelper.ShowLogo());
        GUI.DrawWindow("Matching products", 0, 10, outputData, 120);

        int index = InputHelper.PromptInt("\nEnter the number of the product you want to view the details of", 1, results.Count,
            $"Please enter a number from 1 to {results.Count}");

        var selectedProduct = results[index - 1];
        ShowProductDetails(selectedProduct);
    }

    public void ShowProductDetails(Product product)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.CursorVisible = false;

        var outputData = new List<string>
    {
        $"Name:             {product.Name}",
        $"Description:      {product.Description}",
        $"Price:            {product.Price:C}",
        $"Stock:            {product.Stock}",
        $"Category:         {product.Category.Name}",
        $"Supplier:         {product.Supplier.Name}",
        $"Featured Deal:    {(product.IsFeatured == true ? "Yes" : "No")}"
    };

        GUI.DrawWindow("Product Details", 20, 10, outputData, 80);

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
                    Console.CursorVisible = true;
                    if (product.Stock <= 0)
                    {
                        Console.Clear();
                        OutputHelper.ShowError("Sorry, the product is out of stock!");
                        waitingForValidInput = false;
                        break;
                    }

                    int available = _cartService.GetAvailableToAdd(product);

                    if (available <= 0)
                    {
                        OutputHelper.ShowError("You've already added the maximum available stock.");
                        waitingForValidInput = false;
                        break;
                    }

                    int quantity = InputHelper.PromptInt($"Enter quantity to add to cart", 1, available, $"Please enter a number from 1 to {available}");

                    if (_cartService.TryAddToCart(product, quantity, out string message))
                    {
                        OutputHelper.ShowSuccess(message);
                        waitingForValidInput = false;
                    }
                    else
                    {
                        OutputHelper.ShowError(message);
                    }
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

    public async Task ShowDealsAsync()
    {
        var deals = await db.Products
        .Where(product => product.IsFeatured == true)
        .Take(3)
        .ToListAsync();

        for (int i = 0; i < 3; i++)
        {
            var deal = deals.ElementAtOrDefault(i);
            int fromLeft = i == 0 ? 1 : i == 1 ? 41 : 81;
            int fromTop = 19;
            int maxWidth = 35;
            string dealBuyKeyMessage = deal?.Stock > 0 ? (i == 0 ? "Press X to buy" : i == 1 ? "Press A to buy" : "Press Z to buy") : "Out of stock!";

            if (deal != null)
            {
                GUI.DrawWindow($"Deal {i + 1}", fromLeft, fromTop, new List<string>
            {
                deal.Name,
                deal.Description,
                $"{deal.Price:C}",
                $"{deal.Stock} pieces left in stock!",
                "",
                dealBuyKeyMessage
            }, maxWidth);
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

    public async Task AddProductAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var category = await CategorySelector.ChooseCategoryAsync(db);
        if (category == null) return;
        int categoryId = category.Id;

        var supplier = await SupplierSelector.ChooseSupplierAsync(db);
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

        await db.Products.AddAsync(product);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
          OutputHelper.ShowSuccess("The product was added to the database!");
          await LogHelper.LogAsync(db, "NewProduct", $"A new product was added in the category '{product.Category.Name}': {product.Name}");
        }
    }


    public async Task EditProductAsync()
    {
        if (!RequireAdminAccess()) return;

        var product = await ProductSelector.ChooseProductFromCategoryAsync(db);
        if (product == null) return;

        string oldName = product.Name;
        string oldDesc = product.Description;
        decimal oldPrice = product.Price;
        int oldStock = product.Stock;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo($"\n=== Editing '{product.Name}' ===\n");

        string? newName = ProductValidator.PromptOptionalName(oldName);
        if (!string.IsNullOrWhiteSpace(newName))
            product.Name = newName;

        string? newDesc = ProductValidator.PromptOptionalDescription(oldDesc);
        if (!string.IsNullOrWhiteSpace(newDesc))
            product.Description = newDesc;

        decimal? newPrice = ProductValidator.PromptOptionalPrice(oldPrice);
        if (newPrice.HasValue)
            product.Price = newPrice.Value;

        int? newStock = ProductValidator.PromptOptionalStock(oldStock);
        if (newStock.HasValue)
            product.Stock = newStock.Value;

        Console.WriteLine($"\nShould the product be displayed as an offer?");
        Console.WriteLine($"Currently it {(product.IsFeatured == true ? "IS set to an offer" : "is NOT set to an offer")}");
        product.IsFeatured = ProductValidator.PromptConfirmation();

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
            OutputHelper.ShowSuccess("The product was updated successfully!");
            await LogHelper.LogAsync(
            db,
            "EditProduct",
            $"Product edited: {oldName} to {(string.IsNullOrWhiteSpace(newName)|| newName == oldName ? "Unchanged" : newName)}, " +
            $"Price: {oldPrice:C} to {(newPrice.HasValue && newPrice.Value != oldPrice ? $"{newPrice.Value:C}" : "Unchanged")}, " +
            $"Description: {oldDesc} to {(string.IsNullOrWhiteSpace(newDesc) || newDesc == oldDesc ? "Unchanged" : newDesc)}, " +
            $"Stock: {oldStock} pcs to {(newStock.HasValue && newStock.Value != oldStock ? newStock.Value + " pcs" : "Unchanged")}"
        );
        }
    }

    public async Task DeleteProductAsync()
    {
        if (!RequireAdminAccess()) return;

        var product = await ProductSelector.ChooseProductFromCategoryAsync(db);
        if (product == null) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.WriteLine($"\nAre you sure you want to delete '{product.Name}'?");

        if (!ProductValidator.PromptConfirmation())
        {
            OutputHelper.ShowInfo("Cancelled!");
            return;
        }

        db.Products.Remove(product);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
            OutputHelper.ShowSuccess("The product was deleted successfully!");
            await LogHelper.LogAsync(db, "DeleteProduct", $"Product deleted: {product.Name}");
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
