using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NoFallZone.Services;

namespace NoFallZone.Menu
{
    public class MainMenu
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public MainMenu(IProductService productService, ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }

        public void Show()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                GUI.DrawWindow("=== NoFallZone ===", 43, 0, new List<string> { "Your #1 Source Of Climbing Gear!" });               
                Console.WriteLine();

                GUI.DrawWindow("Main Menu", 0, 5, new List<string>
                {
                    "1. Show All Products",
                    "2. Add Products",
                    "3. Edit Products",
                    "4. Delete Products",
                    "5. Exit",
                    "6. Add Customer"
                });

                _productService.ShowDeals();

                var input = Console.ReadKey(true).Key;

                switch (input)
                {
                    case ConsoleKey.D1:
                        _productService.ShowAllProducts();
                        break;
                    case ConsoleKey.D2:
                        _productService.AddProduct();
                        break;
                    case ConsoleKey.D3:
                        _productService.EditProduct();
                        break;
                    case ConsoleKey.D4:
                        _productService.DeleteProduct();
                        break;
                    case ConsoleKey.X:
                        Console.Clear();
                        Console.WriteLine("This adds deal number 1 to the cart!");
                        break;
                    case ConsoleKey.A:
                        Console.Clear();
                        Console.WriteLine("This adds deal number 2 to the cart!");
                        break;
                    case ConsoleKey.Z:
                        Console.Clear();
                        Console.WriteLine("This adds deal number 3 to the cart!");
                        break;
                    case ConsoleKey.D5:
                        Console.Clear();
                        Console.WriteLine("Thank you for visiting NoFallZone! Laters!");
                        running = false;
                        break;
                    case ConsoleKey.D6:
                        _customerService.AddCustomer();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid choice, please try again!");
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