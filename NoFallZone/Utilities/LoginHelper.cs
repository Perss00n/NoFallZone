using NoFallZone.Data;
using NoFallZone.Models;

namespace NoFallZone.Utilities;

public static class LoginHelper
{
    public static Customer? LoginUser(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine("=== Login ===");

        Console.Write("Username: ");
        string username = Console.ReadLine()!.Trim();

        Console.Write("Password: ");
        string password = Console.ReadLine()!.Trim();

        var customer = db.Customers
            .FirstOrDefault(c => c.Username == username && c.Password == password);

        if (customer == null)
        {
            Console.Clear();
            OutputHelper.ShowError("Login failed! Incorrect username or password");
            Thread.Sleep(1500);
            return null;
        }

        Console.Clear();
        OutputHelper.ShowInfo($"Welcome back, {customer.FullName}! You are now logged in as a {customer.Role}");
        Thread.Sleep(2500);
        return customer;
    }
}
