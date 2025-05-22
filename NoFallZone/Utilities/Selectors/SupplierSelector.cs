using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public static class SupplierSelector
{
    public static Supplier? ChooseSupplier(NoFallZoneContext db)
    {
        Console.Clear();
        Console.CursorVisible = true;
        var suppliers = db.Suppliers.ToList();

        if (suppliers.Count == 0)
        {
            OutputHelper.ShowError("No suppliers found! Returning to main menu...");
            return null;
        }
        var lines = new List<string>();

        for (int i = 0; i < suppliers.Count; i++)
            lines.Add($"{i + 1}. {suppliers[i].Name}");

        GUI.DrawWindow("Choose a supplier", 1, 1, lines, maxLineWidth: 100);

        int index = InputHelper.PromptInt("\nEnter supplier number", 1, suppliers.Count,
            $"Please enter a number from 1 to {suppliers.Count}");

        return suppliers[index - 1];
    }
}
