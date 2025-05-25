using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Utilities.Validators;

namespace NoFallZone.Services.Implementations;
public class PaymentOptionService : IPaymentOptionService
{
    private readonly NoFallZoneContext db;

    public PaymentOptionService(NoFallZoneContext context)
    {
        db = context;
    }

    public async Task ShowAllPaymentOptionsAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        var paymentOptions = await db.PaymentOptions.ToListAsync();

        List<string> outputData;

        if (paymentOptions.Count == 0)
        {
            outputData = new List<string>
        {
            "No payment options found in the database!"
        };
        }
        else
        {
            outputData = paymentOptions.Select(p =>
                $"Id: {p.Id} || Name: {p.Name} || Fee: {p.Fee:C}"
            ).ToList();
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        GUI.DrawWindow("Payment Options", 1, 10, outputData, 100);
    }

    public async Task AddPaymentOptionAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        string paymentName = PaymentOptionValidator.PromptName();
        decimal paymentFee = PaymentOptionValidator.PromptFee();

        var newPaymentOption = new PaymentOption()
        {
            Name = paymentName,
            Fee = paymentFee
        };

        await db.PaymentOptions.AddAsync(newPaymentOption);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
            OutputHelper.ShowSuccess("The payment option has been added to the database!");
            await LogHelper.LogAsync(db, "AddPaymentOption", $"New payment option added: {newPaymentOption.Name}");
        }
    }

    public async Task EditPaymentOptionAsync()
    {
        if (!RequireAdminAccess()) return;

        var paymentOption = await PaymentSelector.ChoosePaymentOptionAsync(db);
        if (paymentOption == null) return;

        string oldName = paymentOption.Name;
        decimal oldFee = paymentOption.Fee;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        string? newName = PaymentOptionValidator.PromptOptionalName(oldName);
        if (!string.IsNullOrWhiteSpace(newName))
            paymentOption.Name = newName;

        decimal? newFee = PaymentOptionValidator.PromptOptionalFee(oldFee);
        if (newFee.HasValue)
            paymentOption.Fee = newFee.Value;

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
            OutputHelper.ShowSuccess("The payment option has been updated successfully!");
            await LogHelper.LogAsync(
            db,
            "EditPaymentOption",
            $"Payment option edited: {oldName} to {(string.IsNullOrWhiteSpace(newName) || newName == oldName ? "Unchanged" : newName)}, " +
            $"Price: {oldFee:C} to {(newFee.HasValue && newFee.Value != oldFee ? $"{newFee.Value:C}" : "Unchanged")}"
        );
        }
    }

    public async Task DeletePaymentOptionAsync()
    {
        if (!RequireAdminAccess()) return;

        var paymentOption = await PaymentSelector.ChoosePaymentOptionAsync(db);
        if (paymentOption == null) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.WriteLine($"Are you sure you want to delete the payment option '{paymentOption.Name}'?");
        if (!PaymentOptionValidator.PromptConfirmation())
        {
            OutputHelper.ShowInfo("Cancelled!");
            return;
        }

        db.PaymentOptions.Remove(paymentOption);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
            OutputHelper.ShowSuccess("The payment option has been deleted successfully!");
            await LogHelper.LogAsync(db, "DeletePaymentOption", $"Payment option deleted: {paymentOption.Name}");
        }
    }

    private bool RequireAdminAccess()
    {
        if (!Session.IsAdmin)
        {
            OutputHelper.ShowError("Access Denied!");
            return false;
        }
        return true;
    }
}