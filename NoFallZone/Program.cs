using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Services;
using NoFallZone.Utilities;

namespace NoFallZone
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new NoFallZoneContext();
            //SeedData.ClearDatabase(db);
            //SeedData.Initialize(db);
            IProductService productService = new ProductService(db);
            ICustomerService customerService = new CustomerService(db);

            AdminMenu adminMenu = new AdminMenu(productService, customerService);
            CustomerMenu customerMenu = new CustomerMenu(productService, customerService);

            bool running = true;

            while (running)
            {
                Console.Clear();
                GUI.DrawWindow("Welcome to NoFallZone", 30, 2, new List<string>
            {
                "[1] Log in as Customer",
                "[2] Log in as Admin",
                "[3] Exit"
            });

                var choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.D1:
                        var customerStart = new StartPage(productService, customerService, customerMenu, null);
                        customerStart.Show();
                        break;
                    case ConsoleKey.D2:
                        var adminStart = new StartPage(productService, customerService, customerMenu, adminMenu);
                        adminStart.Show();
                        break;
                    case ConsoleKey.D3:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Try again.");
                        Thread.Sleep(1000);
                        break;
                }
            }

            Console.Clear();
            Console.WriteLine("Thank you for visiting NoFallZone!");

        }
    }
}