using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFallZone.Services;
using NoFallZone.Utilities;

namespace NoFallZone.Menu;
public class StartPage
{
    private readonly IProductService _productService;
    private readonly ICustomerService _customerService;
    private readonly ICategoryService _categoryService;
    private readonly ISupplierService _supplierService;
    private readonly CustomerMenu? _customerMenu;
    private readonly AdminMenu? _adminMenu;

    public StartPage(IProductService productService, ICustomerService customerService, ICategoryService categoryService, ISupplierService supplierService, CustomerMenu? customerMenu, AdminMenu? adminMenu)
    {
        _productService = productService;
        _customerService = customerService;
        _categoryService = categoryService;
        _supplierService = supplierService;
        _customerMenu = customerMenu;
        _adminMenu = adminMenu;
    }

    public void Show()
    {
        bool inSession = true;

        while (inSession)
        {
            Console.Clear();

            GUI.DrawWindow("=== NoFallZone ===", 43, 0, new List<string> {
            "Your #1 Source Of Climbing Gear!",
            "",
            $"Welcome back, {Session.GetDisplayNameAndRole()}",
            "To Logout Press 'Q'"
        });

            if (_customerMenu != null)
                GUI.DrawWindow("Customer Menu", 0, 8, _customerMenu.GetMenuItems());

            if (Session.IsAdmin && _adminMenu != null)
                GUI.DrawWindow("Admin Menu", 45, 8, _adminMenu.GetMenuItems());

            _productService.ShowDeals();

            var input = Console.ReadKey(true).Key;
            bool isValidChoice = false;

            if (_customerMenu != null)
            {
                switch (input)
                {
                    case ConsoleKey.H: _customerMenu.GoHome(); isValidChoice = true; break;
                    case ConsoleKey.E: _customerMenu.ShowShop(); isValidChoice = true; break;
                    case ConsoleKey.C: _customerMenu.OpenCart(); isValidChoice = true; break;
                    case ConsoleKey.S: _customerMenu.Search(); isValidChoice = true; break;
                }
            }

            if (Session.IsAdmin && _adminMenu != null)
            {
                switch (input)
                {
                    case ConsoleKey.D1: _adminMenu.ShowProductAdminMenu(); isValidChoice = true; break;
                    case ConsoleKey.D2: _adminMenu.ShowCategoryAdminMenu(); isValidChoice = true; break;
                    case ConsoleKey.D3: _adminMenu.ShowCustomerAdminMenu(); isValidChoice = true; break;
                    case ConsoleKey.D4: _adminMenu.ShowSupplierAdminMenu(); isValidChoice = true; break;
                }
            }

            if (input == ConsoleKey.Q)
            {
                Session.Logout();
                OutputHelper.ShowInfo("You have been logged out.");
                Thread.Sleep(1000);
                return;
            }

            if (!isValidChoice)
            {
                OutputHelper.ShowError("Invalid choice!");
            }

            if (inSession)
            {
                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

}
