using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Validators;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Session;
using NoFallZone.Models.Enums;
using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Implementations;
public class CustomerService : ICustomerService
{
    private readonly NoFallZoneContext db;

    public CustomerService(NoFallZoneContext context)
    {
        db = context;
    }


    public void ShowAllCustomers()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== All Customers ===\n");

        var customers = db.Customers.ToList();

        if (customers.Count == 0)
        {
            GUI.DrawWindow("Customers", 1, 2, new List<string>
        {
            "No customers found in the database."
        }, maxLineWidth: 70);
            return;
        }

        int fromTop = 2;
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
        }, maxLineWidth: 70);

            fromTop += 13;
        }
    }


    public void AddCustomer()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
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

        db.Customers.Add(customer);
        db.SaveChanges();

        OutputHelper.ShowSuccess("Account successfully added to the database!");
    }

    public void EditCustomer()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== Edit Customer ===");

        var customer = CustomerSelector.ChooseCustomer(db);
        if (customer == null)
        {
            OutputHelper.ShowError("No customer selected! Returning to main menu...");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine($"=== Editing customer: {customer.FullName} ===");

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


        db.SaveChanges();

        OutputHelper.ShowSuccess("Customer updated successfully!");
    }

    public void DeleteCustomer()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        Console.WriteLine("=== Delete a customer ===");

        var customer = CustomerSelector.ChooseCustomer(db);

        if (customer == null)
        {
            OutputHelper.ShowError("Returning to main menu...");
            return;
        }

        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete the user '{customer.FullName}'?");
        bool confirm = CustomerValidator.PromptConfirmation();

        if (confirm)
        {
            db.Customers.Remove(customer);
            db.SaveChanges();

            OutputHelper.ShowSuccess("User deleted successfully!");
        }
        else
        {
            OutputHelper.ShowError("Deletion cancelled! Returning to main menu...");
        }
    }

    public void ShowCart()
    {
        if (Session.Cart.Count == 0)
        {
            GUI.DrawWindow("Your Cart", 78, 1, new List<string>
            {
                "Your cart is empty."
            });
            return;
        }

        decimal total = 0;
        var lines = new List<string>();

        for (int i = 0; i < Session.Cart.Count; i++)
        {
            var item = Session.Cart[i];
            decimal itemTotal = item.Product.Price * item.Quantity;
            total += itemTotal;

            lines.Add($"{item.Quantity} x {item.Product.Name}");
        }

        lines.Add("------------------------");
        lines.Add($"Total: {total:C}");
        lines.Add("Press 'K' to checkout");
        GUI.DrawWindow("Your Cart", 78, 1, lines, maxLineWidth: 50);
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
