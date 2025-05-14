using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFallZone.Services;

namespace NoFallZone.Menu;
public class StartPage
{
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly CustomerMenu? _customerMenu;
    private readonly AdminMenu? _adminMenu;

    public StartPage(IProductService productService, ICustomerService customerService, CustomerMenu? customerMenu, AdminMenu? adminMenu)
    {
        _productService = productService;
        _customerService = customerService;
        _customerMenu = customerMenu;
        _adminMenu = adminMenu;
    }

    public void Show()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            GUI.DrawWindow("=== NoFallZone ===", 43, 0, new List<string> { "Your #1 Source Of Climbing Gear!" });

            if (_customerMenu != null)
                GUI.DrawWindow("Customer Menu", 0, 4, _customerMenu.GetMenuItems());

            if (_adminMenu != null)
                GUI.DrawWindow("Admin Menu", 45, 4, _adminMenu.GetMenuItems());

            _productService.ShowDeals();

            Console.WriteLine("\n\nChoose an option or press Q to quit:");
            var input = Console.ReadKey(true).Key;

            if (_adminMenu != null)
            {
                switch (input)
                {
                    case ConsoleKey.D1: _adminMenu.ShowProductAdminMenu(); break;
                    case ConsoleKey.D2: _adminMenu.ShowCategoryAdminMenu(); break;
                    case ConsoleKey.D3: _adminMenu.ShowCustomerAdminMenu(); break;
                    case ConsoleKey.D4: _adminMenu.ShowStatisticsMenu(); break;
                    case ConsoleKey.D5: running = false; break;
                }
            }

            if (_customerMenu != null)
            {
                switch (input)
                {
                    case ConsoleKey.H: _customerMenu.GoHome(); break;
                    case ConsoleKey.S: _customerMenu.ShowShop(); break;
                    case ConsoleKey.C: _customerMenu.OpenCart(); break;
                }
            }

            if (input == ConsoleKey.Q)
                running = false;

            if (running)
            {
                Console.WriteLine("\nPress any key to return...");
                Console.ReadKey();
            }
        }
    }
}
