using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Selectors;
public class PaymentSelector
{


    public static PaymentOption? ChoosePaymentOption(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.CursorVisible = true;
        var paymentOptions = db.PaymentOptions.ToList();

        if (paymentOptions.Count == 0)
        {
            OutputHelper.ShowError("No payment options found!");
            return null;
        }
        var lines = new List<string>();

        for (int i = 0; i < paymentOptions.Count; i++)
            lines.Add($"{i + 1}. {paymentOptions[i].Name} (Fee: {paymentOptions[i].Fee:C})");

        GUI.DrawWindow("Choose a payment option", 1, 10, lines, 100);

        int index = InputHelper.PromptInt("\nEnter payment option number", 1, paymentOptions.Count,
            $"Please enter a number from 1 to {paymentOptions.Count}");

        return paymentOptions[index - 1];
    }


}
