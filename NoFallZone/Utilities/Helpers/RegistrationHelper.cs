using NoFallZone.Data;
using NoFallZone.Models.Entities;
using NoFallZone.Models.Enums;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Utilities.Validators;

namespace NoFallZone.Utilities.Helpers;

public static class RegistrationHelper
{
    public static void RegisterNewCustomer(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.CursorVisible = true;
        Console.WriteLine("=== Register New Account ===");

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

        var customer = new Customer
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
            Role = Role.User
        };

        db.Customers.Add(customer);
        db.SaveChanges();

        Console.Clear();
        db.Entry(customer).Reload();
        Session.LoggedInUser = customer;
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("Account created successfully! You are now logged in and redirected to the homepage. Happy Shopping!");
        Console.ReadKey();
    }
}