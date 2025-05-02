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
            $"[{p.Id}] {p.Name} ({p.Category?.Name}) - {p.Price} kr | Lager: {p.Stock}"
        ).ToList();

        GUI.DrawWindow("Produkter", 5, 5, productDetails, maxLineWidth: 100);
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
                    GUI.DrawWindow($"Erbjudande {i + 1}", fromLeft, fromTop, new List<string>
            {
                deal.Name!,
                deal.Description!,
                $"{deal.Price:C}",
                $"{deal.Stock} st kvar i lager"
            }, maxLineWidth: 30);
                }
                else
                {
                    GUI.DrawWindow($"Erbjudande {i + 1}", fromLeft, fromTop, new List<string>
            {
                "No deal available"
            });
                }
            }
    }



}
