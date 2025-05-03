using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
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
                         .Include(p => p.Category)
                         .Include(p => p.Supplier)
                         .ToList();

        var productDetails = products.Select(p =>
            $"[{p.Id}] {p.Name} ({p.Category?.Name}) - {p.Price:C} | Stock: {p.Stock}"
        ).ToList();

        GUI.DrawWindow("Products", 20, 1, productDetails, maxLineWidth: 100);
    }


    public void ShowDeals()
    {

        var deals = db.Products
        .Where(p => p.IsFeatured == true)
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
        string input;
        bool isValidName;
        do
        {
        Console.Write("Enter the name of the product: ");
            input = Console.ReadLine()!;
            isValidName = !String.IsNullOrEmpty(input);

            if (!isValidName)
            {
                Console.WriteLine("The product name can't be empty! Please try again.");
            }
            
        } while (!isValidName);

        string productName = input;
    }



}
