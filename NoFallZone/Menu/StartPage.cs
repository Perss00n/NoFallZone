using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

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

            Console.CursorVisible = false;

            DisplayHelper.ShowWelcomeBanner();

            if (_customerMenu != null)
                DisplayHelper.ShowCustomerMenu(_customerMenu);

            if (Session.IsAdmin && _adminMenu != null)
                DisplayHelper.ShowAdminMenu(_adminMenu);

            DisplayHelper.ShowCustomerDashboard(_productService, _customerService);

            var input = Console.ReadKey(true).Key;

            if (input == ConsoleKey.Q)
            {
                Console.Clear();
                Session.Logout();
                OutputHelper.ShowInfo("You have been logged out.");
                Thread.Sleep(1000);
                return;
            }

            bool isValidChoice = HandleCustomerInput(input) || HandleAdminInput(input);

            if (!isValidChoice)
            {
                Console.Clear() ;
                OutputHelper.ShowError("Invalid choice!");
            }

            OutputHelper.ShowInfo("Press any key to continue...");
            Console.ReadKey();
        }
    }


    private bool HandleCustomerInput(ConsoleKey input)
    {
        if (_customerMenu == null) return false;

        switch (input)
        {
            case ConsoleKey.E: _customerMenu.ShowShop(); return true;
            case ConsoleKey.C: _customerMenu.OpenCart(); return true;
            case ConsoleKey.S: _customerMenu.Search(); return true;
            case ConsoleKey.X:
            case ConsoleKey.A:
            case ConsoleKey.Z:
            _productService.AddDealToCart(input);return true;
            case ConsoleKey.K when Session.Cart.Count > 0: _customerMenu.OpenCart(); return true;
            default: return false;
        }
    }

    private bool HandleAdminInput(ConsoleKey input)
    {
        if (_adminMenu == null || !Session.IsAdmin) return false;

        switch (input)
        {
            case ConsoleKey.D1: _adminMenu.ShowProductAdminMenu(); return true;
            case ConsoleKey.D2: _adminMenu.ShowCategoryAdminMenu(); return true;
            case ConsoleKey.D3: _adminMenu.ShowCustomerAdminMenu(); return true;
            case ConsoleKey.D4: _adminMenu.ShowSupplierAdminMenu(); return true;
            default: return false;
        }
    }

}
