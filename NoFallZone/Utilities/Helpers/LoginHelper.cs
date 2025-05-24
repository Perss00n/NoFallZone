using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Models.Entities;

namespace NoFallZone.Utilities.Helpers;

public static class LoginHelper
{
    public static async Task<Customer?> LoginUserAsync(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.CursorVisible = true;
        Console.WriteLine("=== Login ===");

        Console.Write("Username: ");
        string username = Console.ReadLine()!.Trim();

        Console.Write("Password: ");
        string password = Console.ReadLine()!.Trim();

        var customer = await db.Customers
            .FirstOrDefaultAsync(c => c.Username == username && c.Password == password);

        if (customer == null)
        {
            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            OutputHelper.ShowError("Login failed! Incorrect username or password");
            Thread.Sleep(1500);
            return null;
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("".PadLeft(25) + $"Welcome back {customer.FullName}! You are now logged in as a {customer.Role}");
        Thread.Sleep(2000);
        return customer;
    }
}
