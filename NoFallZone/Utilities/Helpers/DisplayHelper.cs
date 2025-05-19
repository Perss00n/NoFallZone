using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFallZone.Menu;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Utilities.Helpers;
public static class DisplayHelper
{

    public static void ShowStartPage()
    {
        string header = "=== NoFallZone ===";
        int fromLeft = 40;
        int fromTop = 2;
        List<string> lines = new List<string> {
          "[1] Log in To The Shop",
          "[2] Register New Account",
          "[3] Exit"
        };

        Console.Clear();
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

    public static void ShowCustomerDashboard(IProductService productService, ICustomerService customerService)
    {
        productService.ShowDeals();
        ShowCartOverview();
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
        int fromLeft = 35;
        int fromTop = 8;

        GUI.DrawWindow(header, fromLeft, fromTop, adminMenu.GetMenuItems());
    }

    private static void ShowCartOverview()
    {
        string header = "Your Cart";
        int fromLeft = 78;
        int fromTop = 1;

        if (Session.Cart.Count == 0)
        {
            GUI.DrawWindow(header, fromLeft, fromTop, new List<string>
            {
                "Your cart is empty."
            });
            return;
        }

        decimal total = 0;
        var lines = new List<string>();

        for (int i = 0; i < Session.Cart.Count; i++)
        {
            var item = Session.Cart[i];
            decimal itemTotal = item.Product.Price * item.Quantity;
            total += itemTotal;

            lines.Add($"{item.Quantity} x {item.Product.Name}");
        }

        lines.Add("------------------------");
        lines.Add($"Total: {total:C}");
        lines.Add("Press 'K' to checkout");

        GUI.DrawWindow(header, fromLeft, fromTop, lines, maxLineWidth: 50);
    }
}
