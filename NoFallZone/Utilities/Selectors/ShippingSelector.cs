using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public class ShippingSelector
{
    public static ShippingOption? ChooseShipping(NoFallZoneContext db)
    {
        Console.Clear();
        Console.CursorVisible = true;
        var shippingOptions = db.ShippingOptions.ToList();

        if (shippingOptions.Count == 0)
        {
            OutputHelper.ShowError("No shipping options found!");
            return null;
        }

        var lines = new List<string>();

        for (int i = 0; i < shippingOptions.Count; i++)
            lines.Add($"{i + 1}. {shippingOptions[i].Name} || Price: {shippingOptions[i].Price}");

        GUI.DrawWindow("Choose a shipping option", 1, 1, lines, 100);

        int index = InputHelper.PromptInt("\nEnter option number", 1, shippingOptions.Count,
            $"Please enter a number from 1 to {shippingOptions.Count}");

        return shippingOptions[index - 1];
    }

}
