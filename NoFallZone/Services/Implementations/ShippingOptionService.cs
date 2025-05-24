using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Utilities.Validators;

namespace NoFallZone.Services.Implementations;
public class ShippingOptionService : IShippingOptionService
{
    private readonly NoFallZoneContext db;

    public ShippingOptionService(NoFallZoneContext context)
    {
        db = context;
    }

    public void ShowAllShippingOptions()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());


        if (!db.ShippingOptions.Any())
        {
            GUI.DrawWindow("Shipping Options", 1, 10, new List<string>
                {
                    "No shipping options found in the database!"
                });
            return;
        }

        var shippingOptions = db.ShippingOptions.ToList();

        List<string> outputData = shippingOptions.Select(s => $"Id: {s.Id} || Name: {s.Name} || Price: {s.Price:C}").ToList();

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        GUI.DrawWindow("Shipping Options", 1, 10, outputData, 100);
    }

    public void AddShippingOption()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        string shippingName = ShippingOptionValidator.PromptName();
        decimal shippingPrice = ShippingOptionValidator.PromptPrice();

        var newShippingOption = new ShippingOption()
        {
            Name = shippingName,
            Price = shippingPrice
        };

        db.ShippingOptions.Add(newShippingOption);

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The shipping option has been added to the database!");
        else
            OutputHelper.ShowError(errorMsg);
    }

    public void EditShippingOption()
    {
        if (!RequireAdminAccess()) return;

        var shippingOption = ShippingSelector.ChooseShipping(db);

        if (shippingOption == null) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        string? newShippingName = ShippingOptionValidator.PromptOptionalName(shippingOption.Name!);
        if (!string.IsNullOrWhiteSpace(newShippingName))
            shippingOption.Name = newShippingName;

        decimal? newPrice = ShippingOptionValidator.PromptOptionalPrice(shippingOption.Price);
        if (newPrice.HasValue)
            shippingOption.Price = newPrice.Value;

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The shipping option has been updated successfully!");
        else
            OutputHelper.ShowError(errorMsg);
    }

    public void DeleteShippingOption()
    {
        if (!RequireAdminAccess()) return;

        var shippingOption = ShippingSelector.ChooseShipping(db);
        if (shippingOption == null) return;


        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.WriteLine($"Are you sure you want to delete the shipping option '{shippingOption.Name}'?");
        if (!ShippingOptionValidator.PromptConfirmation())
        {
            OutputHelper.ShowInfo("Cancelled!");
            return;
        }

        db.ShippingOptions.Remove(shippingOption);

        if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
            OutputHelper.ShowSuccess("The shipping option has been deleted successfully!");
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
