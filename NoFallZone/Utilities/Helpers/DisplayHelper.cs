﻿using NoFallZone.Menu;
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
                ╚═╝  ╚═══╝ ╚═════╝ ╚═╝     ╚═╝  ╚═╝╚══════╝╚══════╝╚══════╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝" +
                (Session.IsLoggedIn ? "".PadRight(57) + $"YOUR #1 SOURCE FOR CLIMBING GEAR" + "".PadRight(90) + $"Welcome Back, {Session.GetDisplayNameAndRole()}"
                : "".PadRight(65) + "CREATED BY MARCUS LEHM");
    }

    public static void ShowStartPage()
    {
        string header = "=== Welcome To The Shop ===";
        int fromLeft = 15;
        int fromTop = 10;
        List<string> lines = [
          "[1] Log In",
          "[2] Register New Account",
          "[3] Exit"
        ];

        Console.Clear();
        Console.WriteLine(ShowLogo());
        GUI.DrawWindow(header, fromLeft, fromTop, lines);
    }

    public static void ShowSetupPage()
    {
        string header = "=== Database Setup ===";
        int fromLeft = 60;
        int fromTop = 10;
        List<string> lines = [
          "[P]opulate All Tables In DB With Info",
          "[C]lear Database"
        ];

        
        GUI.DrawWindow(header, fromLeft, fromTop, lines);
    }

    public static void ShowCustomerMenu(CustomerMenu customerMenu)
    {
        string header = "Customer Menu";
        int fromLeft = Session.IsUser ? 15 : 2;
        int fromTop = 9;

        GUI.DrawWindow(header, fromLeft, fromTop, customerMenu.GetMenuItems());
    }

    public static void ShowAdminMenu(AdminMenu adminMenu)
    {
        string header = "Admin Menu";
        int fromLeft = 30;
        int fromTop = 9;

        GUI.DrawWindow(header, fromLeft, fromTop, adminMenu.GetMenuItems());
    }
}
