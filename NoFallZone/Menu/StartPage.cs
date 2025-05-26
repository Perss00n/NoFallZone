using NoFallZone.Services.Implementations;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Menu;
public class StartPage
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly CustomerMenu? _customerMenu;
    private readonly AdminMenu? _adminMenu;

    public StartPage(IProductService productService, ICartService cartService, CustomerMenu customerMenu, AdminMenu adminMenu)
    {
        _productService = productService;
        _customerMenu = customerMenu;
        _adminMenu = adminMenu;
        _cartService = cartService;
    }

    public async Task ShowAsync()
    {
        bool inSession = true;

        while (inSession)
        {
            Console.Clear();
            Console.CursorVisible = false;

            Console.WriteLine(DisplayHelper.ShowLogo());
            DisplayHelper.ShowCustomerMenu(_customerMenu!);

            if (Session.IsAdmin)
                DisplayHelper.ShowAdminMenu(_adminMenu!);

            await _productService.ShowDealsAsync();
            _cartService.ShowStartPageCartOverview();

            var input = Console.ReadKey(true).Key;

            if (input == ConsoleKey.Q)
            {
                Console.Clear();
                Session.Logout();
                Console.WriteLine(DisplayHelper.ShowLogo());
                OutputHelper.ShowInfo("".PadRight(40) + "You have been logged out!");
                Thread.Sleep(1000);
                return;
            }

            bool isValidChoice = await HandleCustomerInputAsync(input) || await HandleAdminInputAsync(input);

            if (!isValidChoice)
            {
                Console.Clear();
                OutputHelper.ShowError("Invalid choice!");
            }

            OutputHelper.ShowInfo("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private async Task<bool> HandleCustomerInputAsync(ConsoleKey input)
    {
        switch (input)
        {
            case ConsoleKey.E: await _customerMenu!.ShowShopAsync(); return true;
            case ConsoleKey.C: await _customerMenu!.OpenCartAsync(); return true;
            case ConsoleKey.S: await _customerMenu!.SearchAsync(); return true;
            case ConsoleKey.X:
            case ConsoleKey.A:
            case ConsoleKey.Z:
                await _cartService.AddDealToCartAsync(input); return true;
            case ConsoleKey.K when Session.Cart.Count > 0:
                await _customerMenu!.OpenCartAsync(); return true;
            default: return false;
        }
    }

    private async Task<bool> HandleAdminInputAsync(ConsoleKey input)
    {
        if (!Session.IsAdmin) return false;

        switch (input)
        {
            case ConsoleKey.D1: await _adminMenu!.ShowProductAdminMenuAsync(); return true;
            case ConsoleKey.D2: await _adminMenu!.ShowCategoryAdminMenuAsync(); return true;
            case ConsoleKey.D3: await _adminMenu!.ShowCustomerAdminMenuAsync(); return true;
            case ConsoleKey.D4: await _adminMenu!.ShowSupplierAdminMenuAsync(); return true;
            case ConsoleKey.D5: await _adminMenu!.ShowShippingOptionsAdminMenuAsync(); return true;
            case ConsoleKey.D6: await _adminMenu!.ShowPaymentOptionsAdminMenuAsync(); return true;
            case ConsoleKey.D7: await _adminMenu!.ShowStatisticsAdminMenuAsync(); return true;
            default: return false;
        }
    }
}