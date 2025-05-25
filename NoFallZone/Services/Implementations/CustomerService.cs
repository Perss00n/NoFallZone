using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Models.Enums;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Utilities.Validators;

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

        foreach (var c in customers)
        {
            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());

            var lines = new List<string>
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
        };

            GUI.DrawWindow($"Customer: {c.FullName}", 1, 10, lines, 70);
            Console.WriteLine("\nPress any key to view the next customer...");
            Console.ReadKey(true);
        }
    }

    public async Task ShowOrderHistoryAsync()
    {
        if (!RequireAdminAccess()) return;

        var customer = await CustomerSelector.ChooseCustomerAsync(db);
        if (customer == null) return;

        var orders = await db.Orders
            .Include(order => order.OrderItems)
            .ThenInclude(order => order.Product)
            .Include(order => order.PaymentOption)
            .Include(order => order.ShippingOption)
            .Where(order => order.CustomerId == customer.Id)
            .OrderByDescending(order => order.OrderDate)
            .ToListAsync();

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        if (!orders.Any())
        {
            OutputHelper.ShowInfo($"{customer.FullName} has no orders.");
            return;
        }

        foreach (var order in orders)
        {
            var lines = new List<string>
        {
            $"Order #{order.Id} - {order.OrderDate:G}",
            $"Shipping: {order.ShippingOption!.Name} ({order.ShippingCost:C})",
            $"Payment: {order.PaymentOption!.Name} (Fee: {order.PaymentOption.Fee:C})",
            "Items:"
        };

            foreach (var item in order.OrderItems)
            {
                var dealTag = item.Product.IsFeatured ? "(DEAL)" : "";
                lines.Add($" - {item.Quantity} x {item.Product.Name} ({item.PricePerUnit:C}) {dealTag}");
            }

            lines.Add(new string('-', 50));
            lines.Add($"Total: {order.TotalPrice:C}");

            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            GUI.DrawWindow($"Order for {customer.FullName}", 1, 10, lines, 100);
            Console.WriteLine("\nPress any key to view next order...");
            Console.ReadKey(true);
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
        {
            OutputHelper.ShowSuccess("The customer has been added to the database!");
            await LogHelper.LogAsync(db, "AdminAddUser", $"A admin added the customer: {customer.FullName} with the role: {customer.Role}");
        }
    }

    public async Task EditCustomerAsync()
    {
        if (!RequireAdminAccess()) return;

        Console.Clear();
        var customer = await CustomerSelector.ChooseCustomerAsync(db);
        if (customer == null) return;

        string oldName = customer.FullName;
        string oldEmail = customer.Email;
        string oldPhone = customer.Phone!;
        string oldAddress = customer.Address;
        string oldPostal = customer.PostalCode;
        string oldCity = customer.City;
        string oldCountry = customer.Country;
        int oldAge = customer.Age!.Value;
        string oldUsername = customer.Username;
        string oldPassword = customer.Password;
        Role oldRole = customer.Role;

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo($"=== Editing customer: {oldName} ===\n");

        string? newName = CustomerValidator.PromptOptionalName(oldName);
        if (!string.IsNullOrWhiteSpace(newName)) customer.FullName = newName;

        string? newEmail = CustomerValidator.PromptOptionalEmail(db, oldEmail);
        if (!string.IsNullOrWhiteSpace(newEmail)) customer.Email = newEmail;

        string? newPhone = CustomerValidator.PromptOptionalPhone(oldPhone);
        if (!string.IsNullOrWhiteSpace(newPhone)) customer.Phone = newPhone;

        string? newAddress = CustomerValidator.PromptOptionalAddress(oldAddress);
        if (!string.IsNullOrWhiteSpace(newAddress)) customer.Address = newAddress;

        string? newPostal = CustomerValidator.PromptOptionalPostalCode(oldPostal);
        if (!string.IsNullOrWhiteSpace(newPostal)) customer.PostalCode = newPostal;

        string? newCity = CustomerValidator.PromptOptionalCity(oldCity);
        if (!string.IsNullOrWhiteSpace(newCity)) customer.City = newCity;

        string? newCountry = CustomerValidator.PromptOptionalCountry(oldCountry);
        if (!string.IsNullOrWhiteSpace(newCountry)) customer.Country = newCountry;

        int? newAge = CustomerValidator.PromptOptionalAge(oldAge);
        if (newAge.HasValue) customer.Age = newAge.Value;

        string? newUsername = CustomerValidator.PromptOptionalUsername(db, oldUsername);
        if (!string.IsNullOrWhiteSpace(newUsername)) customer.Username = newUsername;

        string? newPassword = CustomerValidator.PromptOptionalPassword(oldPassword);
        if (!string.IsNullOrWhiteSpace(newPassword)) customer.Password = newPassword;

        Role? newRole = CustomerValidator.PromptOptionalRole(oldRole);
        if (newRole.HasValue) customer.Role = newRole.Value;

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
        {
            OutputHelper.ShowSuccess("Customer updated successfully!");

            await LogHelper.LogAsync(
                db,
                "EditCustomer",
                $"Customer edited: {oldName} to {(string.IsNullOrWhiteSpace(newName) || newName == oldName ? "Unchanged" : newName)}, " +
                $"Email: {oldEmail} to {(string.IsNullOrWhiteSpace(newEmail) || newEmail == oldEmail ? "Unchanged" : newEmail)}, " +
                $"Phone: {oldPhone} to {(string.IsNullOrWhiteSpace(newPhone) || newPhone == oldPhone ? "Unchanged" : newPhone)}, " +
                $"Address: {oldAddress} to {(string.IsNullOrWhiteSpace(newAddress) || newAddress == oldAddress ? "Unchanged" : newAddress)}, " +
                $"PostalCode: {oldPostal} to {(string.IsNullOrWhiteSpace(newPostal) || newPostal == oldPostal ? "Unchanged" : newPostal)}, " +
                $"City: {oldCity} to {(string.IsNullOrWhiteSpace(newCity) || newCity == oldCity ? "Unchanged" : newCity)}, " +
                $"Country: {oldCountry} to {(string.IsNullOrWhiteSpace(newCountry) || newCountry == oldCountry ? "Unchanged" : newCountry)}, " +
                $"Age: {oldAge} to {(newAge.HasValue && newAge.Value != oldAge ? newAge.Value : "Unchanged")}, " +
                $"Username: {oldUsername} to {(string.IsNullOrWhiteSpace(newUsername) || newUsername == oldUsername ? "Unchanged" : newUsername)}, " +
                $"Password: {(newPassword == oldPassword || string.IsNullOrWhiteSpace(newPassword) ? "Unchanged" : "Changed")}, " +
                $"Role: {oldRole} to {(newRole.HasValue && newRole.Value != oldRole ? newRole.Value : "Unchanged")}"
            );
        }
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
        {
            OutputHelper.ShowSuccess("The customer was deleted successfully!");
            await LogHelper.LogAsync(db, "DeleteUser", $"Customer deleted: {customer.FullName}");
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
