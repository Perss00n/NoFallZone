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

        var shippingOptions = db.ShippingOptions.ToList();

        if (shippingOptions.Count == 0)
        {
            GUI.DrawWindow("Shipping Options", 1, 2, new List<string>
                {
                    "No shipping options found in the database!"
                });
            return;
        }

        List<string> outputData = shippingOptions.Select(s => $"Id: {s.Id} || Name: {s.Name} || Price: {s.Price:C}").ToList();

        GUI.DrawWindow("Shipping Options", 1, 2, outputData, maxLineWidth: 100);
    }

    public void AddShippingOption()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== Add a new shipping option ===");

        string shippingName = ShippingOptionValidator.PromptName();
        decimal shippingPrice = ShippingOptionValidator.PromptPrice();

        var newShippingOption = new ShippingOption()
        {
            Name = shippingName,
            Price = shippingPrice
        };

        db.ShippingOptions.Add(newShippingOption);
        db.SaveChanges();

        OutputHelper.ShowSuccess("The Shipping option has been added to the database!");
    }

    public void EditShippingOption()
    {
        if (!RequireAdminAccess()) return;

        var shippingOption = ShippingSelector.ChooseShipping(db);

        if (shippingOption == null) return;

        string? newShippingName = ShippingOptionValidator.PromptOptionalName(shippingOption.Name!);
        if (!string.IsNullOrWhiteSpace(newShippingName))
            shippingOption.Name = newShippingName;

        decimal? newPrice = ShippingOptionValidator.PromptOptionalPrice(shippingOption.Price);
        if (newPrice.HasValue)
            shippingOption.Price = newPrice.Value;

        db.SaveChanges();

        OutputHelper.ShowSuccess("Shipping option updated successfully!");
    }

    public void DeleteShippingOption()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== Delete a shipping option ===");

        var shippingOption = ShippingSelector.ChooseShipping(db);
        if (shippingOption == null) return;

        Console.Clear();

        Console.WriteLine($"Are you sure you want to delete the shipping option '{shippingOption.Name}'?");
        bool confirm = ShippingOptionValidator.PromptConfirmation();

        if (confirm)
        {
            db.ShippingOptions.Remove(shippingOption);
            db.SaveChanges();

            OutputHelper.ShowSuccess("Shipping option deleted successfully!");
        }
        else
        {
            OutputHelper.ShowError("Deletion cancelled!");
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
