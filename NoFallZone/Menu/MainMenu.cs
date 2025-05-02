using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFallZone.Services;

namespace NoFallZone.Menu
{
    public class MainMenu
    {
        private readonly ProductService _productService;

        public MainMenu(ProductService productService)
        {
            _productService = productService;
        }

        public void Show()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                GUI.DrawWindow("", 2, 1, new List<string> { "=== NoFallZone ===", "Your No. 1 Source Of Climbing Gear!" });               
                Console.WriteLine();
                Console.WriteLine("=== NoFallZone – Huvudmeny ===");
                Console.WriteLine("1. Visa produkter");
                Console.WriteLine("2. Visa senaste ordern");
                Console.WriteLine("3. Avsluta");
                Console.Write("Val: ");
                _productService.ShowDeals();

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        _productService.ShowAllProducts();
                        break;
                    case "2":
                        //_orderService.ShowLatestPurchase();
                        break;
                    case "3":
                        Console.Clear();
                        Console.WriteLine("Tack för att du besökte NoFallZone! På Återseende!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen!");
                        break;
                }

                if (running)
                {
                    Console.Write("\nTryck på valfri tangent för att fortsätta...");
                    Console.ReadKey();
                }
            }

        }
    }
}