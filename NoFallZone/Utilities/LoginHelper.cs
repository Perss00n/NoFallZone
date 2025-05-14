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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Login failed! Incorrect username or password.");
            Console.ResetColor();
            Thread.Sleep(1500);
            return null;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\nWelcome back, {customer.FullName}! You are logged in as {customer.Role}.");
        Console.ResetColor();
        Thread.Sleep(2500);
        return customer;
    }
}
