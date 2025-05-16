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
                $"Welcome back, {Session.GetDisplayNameAndRole()}"
            });

            if (Session.IsLoggedIn && _customerMenu != null)
                GUI.DrawWindow("Customer Menu", 0, 5, _customerMenu.GetMenuItems());

            if (Session.IsAdmin && _adminMenu != null)
                GUI.DrawWindow("Admin Menu", 45, 5, _adminMenu.GetMenuItems());

            _productService.ShowDeals();

            var input = Console.ReadKey(true).Key;

            if (Session.IsAdmin && _adminMenu != null)
            {
                switch (input)
                {
                    case ConsoleKey.D1: _adminMenu.ShowProductAdminMenu(); break;
                    case ConsoleKey.D2: _adminMenu.ShowCategoryAdminMenu(); break;
                    case ConsoleKey.D3: _adminMenu.ShowCustomerAdminMenu(); break;
                    case ConsoleKey.D4: _adminMenu.ShowSupplierAdminMenu(); break;
                    case ConsoleKey.D5: inSession = false; break;
                }
            }

            if (Session.IsUser && _customerMenu != null)
            {
                switch (input)
                {
                    case ConsoleKey.H: _customerMenu.GoHome(); break;
                    case ConsoleKey.S: _customerMenu.ShowShop(); break;
                    case ConsoleKey.C: _customerMenu.OpenCart(); break;
                }
            }

            if (input == ConsoleKey.Q)
                inSession = false;

            if (inSession)
            {
                OutputHelper.ShowInfo("Press any key to return...");
                Console.ReadKey();
            }
        }
    }
}
