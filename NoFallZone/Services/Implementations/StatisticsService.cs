using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Selectors;

namespace NoFallZone.Services.Statistics;

public class StatisticsService : IStatisticsService
{
    private readonly NoFallZoneContext db;

    public StatisticsService(NoFallZoneContext context)
    {
        db = context;
    }

    public async Task ShowMostSoldProductsAsync()
    {
        var topProducts = await db.OrderItems
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .GroupBy(oi => oi.ProductId)
            .Select(g => new
            {
                ProductName = g.First().Product.Name,
                TotalSold = g.Sum(oi => oi.Quantity),
                CategoryName = g.First().Product.Category.Name
            })
            .OrderByDescending(p => p.TotalSold)
            .ThenBy(p => p.ProductName)
            .Take(10)
            .ToListAsync();


        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Top 5 Best Selling Products:\n");

        if (topProducts.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        for (int i = 0; i < topProducts.Count; i++)
            Console.WriteLine($"{i + 1}. {topProducts[i].ProductName} ({topProducts[i].CategoryName}): {topProducts[i].TotalSold} sold");

    }

    public async Task ShowTopSellingProductsInCategoryAsync()
    {
        var category = await CategorySelector.ChooseCategoryAsync(db);

        if (category == null) return;

        var topProducts = await db.OrderItems
            .Where(oi => oi.Product.CategoryId == category.Id)
            .GroupBy(oi => oi.Product)
            .Select(g => new
            {
                ProductName = g.Key.Name,
                TotalSold = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(p => p.TotalSold)
            .Take(10)
            .ToListAsync();

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo($"Top selling products in category: {category.Name}\n");

        if (topProducts.Count == 0)
        {
            Console.WriteLine("No products have been sold in this category yet.");
            return;
        }

        for (int i = 0; i < topProducts.Count; i++)
            Console.WriteLine($"{i + 1} {topProducts[i].ProductName}: {topProducts[i].TotalSold} sold");
    }

    public async Task ShowTopCategoriesAsync()
    {
        var topCategories = await db.OrderItems
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Category)
            .GroupBy(oi => oi.Product.Category.Name)
            .Select(g => new
            {
                Category = g.Key,
                TotalSold = g.Sum(oi => oi.Quantity)
            })
            .OrderByDescending(c => c.TotalSold)
            .ThenBy(p => p.Category)
            .Take(10)
            .ToListAsync();


        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Top 5 Best Selling Categories:\n");

        if (topCategories.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        for (int i = 0; i < topCategories.Count; i++)
            Console.WriteLine($"{i + 1}. {topCategories[i].Category}: {topCategories[i].TotalSold} {(topCategories[i].TotalSold > 1 ? "items" : "item")} sold");
    }

    public async Task ShowDealSalesCountAsync()
    {
        var count = await db.OrderItems
            .Where(oi => oi.Product.IsFeatured == true)
            .SumAsync(oi => oi.Quantity);


        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        if (count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        OutputHelper.ShowInfo($"Total 'Deal' Products Sold: {count}");
    }

    public async Task ShowOrdersByCityAsync()
    {
        var ordersByCity = await db.Orders
            .Include(o => o.Customer)
            .GroupBy(o => o.Customer.City)
            .Select(g => new
            {
                City = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.City)
            .ToListAsync();


        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Orders Per City:\n");

        if (ordersByCity.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        for (int i = 0; i < ordersByCity.Count; i++)
            Console.WriteLine($"{i + 1}. {ordersByCity[i].City}: {ordersByCity[i].Count} {(ordersByCity[i].Count > 1 ? "orders" : "order")}");

    }

    public async Task ShowSalesBySupplierAsync()
    {
        var stats = await db.OrderItems
            .Include(oi => oi.Product)
            .ThenInclude(p => p.Supplier)
            .GroupBy(oi => oi.Product.Supplier.Name)
            .Select(g => new
            {
                Supplier = g.Key,
                Total = g.Sum(x => x.Quantity * x.PricePerUnit)
            })
            .OrderByDescending(g => g.Total)
            .ThenBy(s => s.Supplier)
            .ToListAsync();


        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Total Sales By Suppliers:\n");

        if (stats.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        for (int i = 0; i < stats.Count; i++)
            Console.WriteLine($"{i + 1}. {stats[i].Supplier}: {stats[i].Total:C}");
    }

    public async Task ShowTopSearchKeywordsAsync()
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var keywords = await db.ActivityLogs
            .Where(log => log.Action == "NewProductSearch")
            .Select(log => log.Details.Trim().ToLower())
            .ToListAsync();

        var topKeywords = keywords
            .GroupBy(k => k)
            .Select(g => new { Keyword = g.Key, Count = g.Count() })
            .OrderByDescending(k => k.Count)
            .ThenBy(s => s.Keyword)
            .Take(10)
            .ToList();


        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Top 5 Most Searched Keywords:\n");

        if (topKeywords.Count == 0)
        {
            OutputHelper.ShowInfo("No search keywords have been logged yet.");
            return;
        }

        for (var i = 0; i < topKeywords.Count; i++)
            Console.WriteLine($"{i + 1}. '{topKeywords[i].Keyword}' has been searched for {topKeywords[i].Count} {(topKeywords[i].Count > 1 ? "times" : "time")}");
    }

    public async Task ShowMostCommonPaymentMethodAsync()
    {
        var result = await db.Orders
            .Include(o => o.PaymentOption)
            .GroupBy(o => o.PaymentOption!.Name)
            .Select(g => new
            {
                PaymentMethod = g.Key,
                UsageCount = g.Count()
            })
            .OrderByDescending(p => p.UsageCount)
            .ThenBy(p => p.PaymentMethod)
            .ToListAsync();

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Most Common Payment Methods:\n");

        if (result.Count == 0)
        {
            Console.WriteLine("No payment data available.");
            return;
        }

        for (var i = 0; i < result.Count; i++)
            Console.WriteLine($"{i + 1}. {result[i].PaymentMethod}: {result[i].UsageCount} uses");
    }

    public async Task ShowMostActiveCustomersAsync()
    {
        var result = await db.Orders
            .Include(o => o.Customer)
            .GroupBy(o => o.Customer)
            .Select(g => new
            {
                CustomerName = g.Key.FullName,
                Email = g.Key.Email,
                OrderCount = g.Count()
            })
            .OrderByDescending(c => c.OrderCount)
            .ThenBy(c => c.CustomerName)
            .Take(10)
            .ToListAsync();

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Top 10 Customers With Most Orders:\n");

        if (result.Count == 0)
        {
            Console.WriteLine("No customer orders found.");
            return;
        }

        for (var i = 0; i < result.Count; i++)
            Console.WriteLine($"{i + 1}. {result[i].CustomerName} ({result[i].Email}): {result[i].OrderCount} {(result[i].OrderCount > 1 ? "orders" : "order")}");
    }

    public async Task ShowTopRevenueGeneratingProductsAsync()
    {
        var result = await db.OrderItems
            .Include(oi => oi.Product)
            .GroupBy(oi => oi.Product)
            .Select(g => new
            {
                ProductName = g.Key.Name,
                Revenue = g.Sum(oi => oi.PricePerUnit * oi.Quantity)
            })
            .OrderByDescending(p => p.Revenue)
            .ThenBy(p => p.ProductName)
            .Take(10)
            .ToListAsync();

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Top 10 Revenue-Generating Products:\n");

        if (result.Count == 0)
        {
            Console.WriteLine("No sales data found.");
            return;
        }

        for (var i = 0; i < result.Count; i++)
            Console.WriteLine($"{i + 1}. {result[i].ProductName}: {result[i].Revenue:C} total revenue");
    }
}