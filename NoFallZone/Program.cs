using Microsoft.Extensions.DependencyInjection;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Services.Implementations;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone;

public class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();

        // Registrera DbContexten
        services.AddSingleton<NoFallZoneContext>();

        // Tjänster
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>();

        // Menyer
        services.AddScoped<AdminMenu>();
        services.AddScoped<CustomerMenu>();
        services.AddScoped<StartPage>();

        var provider = services.BuildServiceProvider();
        var db = provider.GetRequiredService<NoFallZoneContext>();

        bool running = true;

        while (running)
        {
            Console.CursorVisible = false;
            DisplayHelper.ShowStartPage();
            var choice = Console.ReadKey(true).Key;

            switch (choice)
            {
                case ConsoleKey.D1:
                    var user = LoginHelper.LoginUser(db);
                    if (user == null) break;

                    Session.LoggedInUser = user;

                    var startPage = provider.GetRequiredService<StartPage>();
                    startPage.Show();
                    break;

                case ConsoleKey.D2:
                    RegistrationHelper.RegisterNewCustomer(db);
                    break;

                case ConsoleKey.D3:
                    running = false;
                    break;

                default:
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice! Please try again...");
                    Thread.Sleep(1500);
                    break;
            }
        }

        Console.Clear();
        OutputHelper.ShowInfo("Thank you for visiting NoFallZone! L8terZ!");
    }
}