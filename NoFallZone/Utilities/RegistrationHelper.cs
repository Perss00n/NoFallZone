using NoFallZone.Data;
using NoFallZone.Models;

namespace NoFallZone.Utilities;

public static class RegistrationHelper
{
    public static void RegisterNewCustomer(NoFallZoneContext db)
    {
        Console.Clear();
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
        OutputHelper.ShowSuccess("Account created successfully! You may now log in on the homepage");
        Thread.Sleep(2000);
    }
}