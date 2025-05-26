using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;

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
            .Take(5)
            .ToListAsync();

        if (topProducts.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Top 5 Best Selling Products:\n");

        for (int i = 0; i < topProducts.Count; i++)
            Console.WriteLine($"{i + 1}. {topProducts[i].ProductName} ({topProducts[i].CategoryName}): {topProducts[i].TotalSold} sold");

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
            .Take(5)
            .ToListAsync();

        if (topCategories.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Top 5 Best Selling Categories:");

        for (int i = 0; i < topCategories.Count; i++)
            Console.WriteLine($"{i + 1}. {topCategories[i].Category}: {topCategories[i].TotalSold} {(topCategories[i].TotalSold > 1 ? "items" : "item")} sold");
    }

    public async Task ShowDealSalesCountAsync()
    {
        var count = await db.OrderItems
            .Where(oi => oi.Product.IsFeatured == true)
            .SumAsync(oi => oi.Quantity);

        if (count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
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

        if (ordersByCity.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("\nOrders Per City:");

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

        if (stats.Count == 0)
        {
            OutputHelper.ShowInfo("No sales has been made yet.");
            return;
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Total Sales By Supplier:");

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

        if (topKeywords.Count == 0)
        {
            OutputHelper.ShowInfo("No search keywords have been logged yet.");
            return;
        }

        OutputHelper.ShowInfo("\nTop 5 Most Searched Keywords:");

        for (var i = 0; i < topKeywords.Count; i++)
            Console.WriteLine($"{i + 1}. '{topKeywords[i].Keyword}' has been searched for {topKeywords[i].Count} {(topKeywords[i].Count > 1 ? "times" : "time")}");
    }

}