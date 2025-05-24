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

    public void ShowAllPaymentOptions()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        if (!db.PaymentOptions.Any())
        {
            GUI.DrawWindow("Payment Options", 1, 10, new List<string>
                {
                    "No payment options found in the database!"
                });
            return;
        }

        var paymentOptions = db.PaymentOptions.ToList();

        List<string> outputData = paymentOptions.Select(p => $"Id: {p.Id} || Name: {p.Name} || Fee: {p.Fee:C}").ToList();

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        GUI.DrawWindow("Payment Options", 1, 10, outputData, 100);
    }

    public void AddPaymentOption()
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

        db.PaymentOptions.Add(newPaymentOption);

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The payment option has been added to the database!");
        else
            OutputHelper.ShowError(errorMsg);
    }

    public void EditPaymentOption()
    {
        if (!RequireAdminAccess()) return;

        var paymentOption = PaymentSelector.ChoosePaymentOption(db);
        if (paymentOption == null) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        string? newName = PaymentOptionValidator.PromptOptionalName(paymentOption.Name!);
        if (!string.IsNullOrWhiteSpace(newName))
            paymentOption.Name = newName;

        decimal? newFee = PaymentOptionValidator.PromptOptionalFee(paymentOption.Fee ?? 0);
        if (newFee.HasValue)
            paymentOption.Fee = newFee;

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The payment option has been updated successfully!");
        else
            OutputHelper.ShowError(errorMsg);
    }

    public void DeletePaymentOption()
    {
        if (!RequireAdminAccess()) return;

        var paymentOption = PaymentSelector.ChoosePaymentOption(db);
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

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The payment option has been deleted successfully!");
        else
            OutputHelper.ShowError(errorMsg);
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