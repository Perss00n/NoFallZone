using NoFallZone.Data;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class SupplierSelector
{
    public static Supplier? ChooseSupplier(NoFallZoneContext db)
    {
        Console.CursorVisible = true;
        var suppliers = db.Suppliers.ToList();

        if (suppliers.Count == 0)
        {
            OutputHelper.ShowError("No suppliers found! Returning to main menu...");
            return null;
        }

        Console.WriteLine("\nChoose a supplier:");
        for (int i = 0; i < suppliers.Count; i++)
            Console.WriteLine($"{i + 1}. {suppliers[i].Name}");

        int index = InputHelper.PromptInt("Enter supplier number", 1, suppliers.Count,
            $"Please enter a number from 1 to {suppliers.Count}");

        return suppliers[index - 1];
    }
}
