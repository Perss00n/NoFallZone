using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Validators;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Models.Enums;
using NoFallZone.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace NoFallZone.Services.Implementations;
public class CustomerService : ICustomerService
{
    private readonly NoFallZoneContext db;

    public CustomerService(NoFallZoneContext context)
    {
        db = context;
    }

    public async Task ShowAllCustomersAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("=== All Customers ===\n");

        var customers = await db.Customers.ToListAsync();

        if (customers.Count == 0)
        {
            GUI.DrawWindow("Customers", 1, 10, new List<string>
        {
            "No customers found in the database."
        }, 70);
            return;
        }

        int fromTop = 10;
        foreach (var c in customers)
        {
            GUI.DrawWindow($"Customer: {c.FullName}", 1, fromTop, new List<string>
        {
            $"ID:         {c.Id}",
            $"Name:       {c.FullName}",
            $"Email:      {c.Email}",
            $"Phone:      {c.Phone}",
            $"Address:    {c.Address}",
            $"PostalCode: {c.PostalCode}",
            $"City:       {c.City}",
            $"Country:    {c.Country}",
            $"Age:        {c.Age}",
            $"Username:   {c.Username}",
            $"Role:       {c.Role}"
        }, 70);

            fromTop += 13;
        }
    }


    public async Task AddCustomerAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.WriteLine("=== Add a new customer ===");

        string name = CustomerValidator.PromptName();
        string email = CustomerValidator.PromptEmail(db);
        string phone = CustomerValidator.PromptPhone();
        string address = CustomerValidator.PromptAddress();
        string postalCode = CustomerValidator.PromptPostalCode();
        string city = CustomerValidator.PromptCity();
        string country = CustomerValidator.PromptCountry();
        int age = CustomerValidator.PromptAge();
        string userName = CustomerValidator.PromptUsername(db);
        string password = CustomerValidator.PromptPassword();
        Role role = CustomerValidator.PromptRole();

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
            Username = userName,
            Password = password,
            Role = role
        };

        await db.Customers.AddAsync(customer);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The customer has been added to the database!");
    }

    public async Task EditCustomerAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        var customer = await CustomerSelector.ChooseCustomerAsync(db);
        if (customer == null) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo($"=== Editing customer: {customer.FullName} ===\n");

        string? newName = CustomerValidator.PromptOptionalName(customer.FullName);
        if (!string.IsNullOrWhiteSpace(newName)) customer.FullName = newName;

        string? newEmail = CustomerValidator.PromptOptionalEmail(db, customer.Email);
        if (!string.IsNullOrWhiteSpace(newEmail)) customer.Email = newEmail;

        string? newPhone = CustomerValidator.PromptOptionalPhone(customer.Phone!);
        if (!string.IsNullOrWhiteSpace(newPhone)) customer.Phone = newPhone;

        string? newAddress = CustomerValidator.PromptOptionalAddress(customer.Address);
        if (!string.IsNullOrWhiteSpace(newAddress)) customer.Address = newAddress;

        string? newPostal = CustomerValidator.PromptOptionalPostalCode(customer.PostalCode);
        if (!string.IsNullOrWhiteSpace(newPostal)) customer.PostalCode = newPostal;

        string? newCity = CustomerValidator.PromptOptionalCity(customer.City);
        if (!string.IsNullOrWhiteSpace(newCity)) customer.City = newCity;

        string? newCountry = CustomerValidator.PromptOptionalCountry(customer.Country);
        if (!string.IsNullOrWhiteSpace(newCountry)) customer.Country = newCountry;

        int? newAge = CustomerValidator.PromptOptionalAge(customer.Age!.Value);
        if (newAge.HasValue) customer.Age = newAge.Value;

        string? newUsername = CustomerValidator.PromptOptionalUsername(db, customer.Username);
        if (!string.IsNullOrWhiteSpace(newUsername)) customer.Username = newUsername;

        string? newPassword = CustomerValidator.PromptOptionalPassword(customer.Password);
        if (!string.IsNullOrWhiteSpace(newPassword)) customer.Password = newPassword;

        Role? newRole = CustomerValidator.PromptOptionalRole(customer.Role);
        if (newRole.HasValue) customer.Role = newRole.Value;


        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("Customer updated successfully!");
    }

    public async Task DeleteCustomerAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        var customer = await CustomerSelector.ChooseCustomerAsync(db);

        if (customer == null) return;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        Console.WriteLine($"\nAre you sure you want to delete the user '{customer.FullName}'?");
        if (!CustomerValidator.PromptConfirmation())
        {
            OutputHelper.ShowInfo("Cancelled!");
            return;
        }
            db.Customers.Remove(customer);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The customer was deleted successfully!");
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
