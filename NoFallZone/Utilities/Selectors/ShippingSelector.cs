using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public class ShippingSelector
{
    public static async Task<ShippingOption?> ChooseShippingAsync(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.CursorVisible = true;

        var shippingOptions = await db.ShippingOptions.ToListAsync();

        if (shippingOptions.Count == 0)
        {
            OutputHelper.ShowError("No shipping options found!");
            return null;
        }

        var lines = new List<string>();

        for (int i = 0; i < shippingOptions.Count; i++)
            lines.Add($"{i + 1}. {shippingOptions[i].Name} || Price: {shippingOptions[i].Price:C}");

        GUI.DrawWindow("Choose a shipping option", 1, 10, lines, 100);

        int index = InputHelper.PromptInt("\nEnter option number", 1, shippingOptions.Count,
            $"Please enter a number from 1 to {shippingOptions.Count}");

        return shippingOptions[index - 1];
    }

}
