using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class CustomerSelector
{
    public static Customer? ChooseCustomer(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.CursorVisible = true;
        var customers = db.Customers.ToList();

        if (customers.Count == 0)
        {
            OutputHelper.ShowError("No customers found in the database!");
            return null;
        }
        var lines = new List<string>();

        for (int i = 0; i < customers.Count; i++)
            lines.Add($"{i + 1}. {customers[i].FullName} ({customers[i].Email})");

        GUI.DrawWindow("Select a customer", 1, 10, lines, 100);

        int index = InputHelper.PromptInt("\nEnter the number of the customer", 1, customers.Count,
            $"Please select a valid number between 1 and {customers.Count}");

        return customers[index - 1];
    }
}
