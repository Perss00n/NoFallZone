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
public class ShippingOptionService : IShippingOptionService
{
    private readonly NoFallZoneContext db;

    public ShippingOptionService(NoFallZoneContext context)
    {
        db = context;
    }

    public async Task ShowAllShippingOptionsAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        List<string> outputData;

        var shippingOptions = await db.ShippingOptions.ToListAsync();

        if (shippingOptions.Count == 0)
        {
            outputData = new List<string> { "No shipping options found in the database!" };
        }
        else
        {
            outputData = shippingOptions.Select(s => $"Id: {s.Id} || Name: {s.Name} || Price: {s.Price:C}").ToList();
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        GUI.DrawWindow("Shipping Options", 1, 10, outputData, 100);
    }

    public async Task AddShippingOptionAsync()
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

        await db.ShippingOptions.AddAsync(newShippingOption);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The shipping option has been added to the database!");
    }

    public async Task EditShippingOptionAsync()
    {
        if (!RequireAdminAccess()) return;

        var shippingOption = await ShippingSelector.ChooseShippingAsync(db);

        if (shippingOption == null) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        string? newShippingName = ShippingOptionValidator.PromptOptionalName(shippingOption.Name!);
        if (!string.IsNullOrWhiteSpace(newShippingName))
            shippingOption.Name = newShippingName;

        decimal? newPrice = ShippingOptionValidator.PromptOptionalPrice(shippingOption.Price);
        if (newPrice.HasValue)
            shippingOption.Price = newPrice.Value;

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The shipping option has been updated successfully!");
    }

    public async Task DeleteShippingOptionAsync()
    {
        if (!RequireAdminAccess()) return;

        var shippingOption = await ShippingSelector.ChooseShippingAsync(db);
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

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The shipping option has been deleted successfully!");
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
