using NoFallZone.Data;
using NoFallZone.Models.Entities;
using NoFallZone.Models.Enums;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Utilities.Validators;

public static class RegistrationHelper
{
    public static async Task RegisterNewCustomerAsync(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("=== Register New Customer ===");

        string name = CustomerValidator.PromptName();
        string email = CustomerValidator.PromptEmail(db);
        string phone = CustomerValidator.PromptPhone();
        string address = CustomerValidator.PromptAddress();
        string postalCode = CustomerValidator.PromptPostalCode();
        string city = CustomerValidator.PromptCity();
        string country = CustomerValidator.PromptCountry();
        int age = CustomerValidator.PromptAge();
        string username = CustomerValidator.PromptUsername(db);
        string password = CustomerValidator.PromptPassword();
        Role role = Role.User;

        var newCustomer = new Customer
        {
            FullName = name,
            Email = email,
            Phone = phone,
            Address = address,
            PostalCode = postalCode,
            City = city,
            Country = country,
            Age = age,
            Username = username,
            Password = password,
            Role = role
        };

        await db.Customers.AddAsync(newCustomer);
        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
            OutputHelper.ShowSuccess("Account created successfully! You have been logged in and are now redirected to the home page. Happy shopping!");
            Session.LoggedInUser = newCustomer;
            await LogHelper.LogAsync(db, "Register", $"New user registration: {newCustomer.FullName}");
            Console.ReadKey();
        }
    }
}