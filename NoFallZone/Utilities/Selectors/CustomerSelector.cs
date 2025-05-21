using NoFallZone.Data;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class CustomerSelector
{
    public static Customer? ChooseCustomer(NoFallZoneContext db)
    {
        Console.CursorVisible = true;
        var customers = db.Customers.ToList();

        if (customers.Count == 0)
        {
            OutputHelper.ShowError("No customers found in the database");
            return null;
        }

        Console.WriteLine("\nSelect a customer:");
        for (int i = 0; i < customers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {customers[i].FullName} ({customers[i].Email})");
        }

        int index = InputHelper.PromptInt("Enter the number of the customer", 1, customers.Count,
            $"Please select a valid number between 1 and {customers.Count}");

        return customers[index - 1];
    }
}
