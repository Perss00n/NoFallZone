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
                GUI.DrawWindow("=== NoFallZone ===", 0, 0, new List<string> { "Your No. 1 Source Of Climbing Gear!" });               
                Console.WriteLine();
                Console.WriteLine("=== NoFallZone – Main Menu ===");
                Console.WriteLine("1. Show All Products");
                Console.WriteLine("2. Show Latest Order");
                Console.WriteLine("3. Exit");
                Console.Write("Choice: ");
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
                        Console.WriteLine("Thank you for visiting NoFallZone! Laters!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Unvalid choice, please try again!");
                        break;
                }

                if (running)
                {
                    Console.Write("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }

        }
    }
}