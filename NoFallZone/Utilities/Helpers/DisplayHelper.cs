using NoFallZone.Menu;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Utilities.Helpers;
public static class DisplayHelper
{


    public static string ShowLogo()
    {
        return @"
                ███╗   ██╗ ██████╗ ███████╗ █████╗ ██╗     ██╗     ███████╗ ██████╗ ███╗   ██╗███████╗
                ████╗  ██║██╔═══██╗██╔════╝██╔══██╗██║     ██║     ╚══███╔╝██╔═══██╗████╗  ██║██╔════╝
                ██╔██╗ ██║██║   ██║█████╗  ███████║██║     ██║       ███╔╝ ██║   ██║██╔██╗ ██║█████╗  
                ██║╚██╗██║██║   ██║██╔══╝  ██╔══██║██║     ██║      ███╔╝  ██║   ██║██║╚██╗██║██╔══╝  
                ██║ ╚████║╚██████╔╝██║     ██║  ██║███████╗███████╗███████╗╚██████╔╝██║ ╚████║███████╗
                ╚═╝  ╚═══╝ ╚═════╝ ╚═╝     ╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝
                                           CREATED BY MARCUS LEHM";
    }

    public static void ShowStartPage()
    {
        string header = "=== Welcome To The Shop ===";
        int fromLeft = 40;
        int fromTop = 10;
        List<string> lines = new List<string> {
          "[1] Log In",
          "[2] Register New Account",
          "[3] Exit"
        };

        Console.Clear();
        Console.WriteLine(ShowLogo());
        GUI.DrawWindow(header, fromLeft, fromTop, lines);
    }

    public static void ShowWelcomeBanner()
    {
        string header = "=== NoFallZone ===";
        int fromLeft = 13;
        int fromTop = 1;

        List<String> lines = new List<String> {
            "Your #1 Source Of Climbing Gear!",
            "",
            $"Welcome back, {Session.GetDisplayNameAndRole()}",
            "To Logout Press 'Q'"
        };

        GUI.DrawWindow(header, fromLeft, fromTop, lines);
    }

    public static void ShowCustomerDashboard(IProductService productService, ICartService cartService)
    {
        productService.ShowDeals();
        cartService.ShowStartPageCartOverview();
    }

    public static void ShowCustomerMenu(CustomerMenu customerMenu)
    {
        string header = "Customer Menu";
        int fromLeft = 2;
        int fromTop = 8;

        GUI.DrawWindow(header, fromLeft, fromTop, customerMenu.GetMenuItems());
    }

    public static void ShowAdminMenu(AdminMenu adminMenu)
    {
        string header = "Admin Menu";
        int fromLeft = 30;
        int fromTop = 8;

        GUI.DrawWindow(header, fromLeft, fromTop, adminMenu.GetMenuItems());
    }
}
