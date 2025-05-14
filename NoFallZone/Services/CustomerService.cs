using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Utilities;
using NoFallZone.Models;

namespace NoFallZone.Services;
public class CustomerService : ICustomerService
{
    private readonly NoFallZoneContext db;

    public CustomerService(NoFallZoneContext context)
    {
        db = context;
    }


    public void ShowAllCustomers()
    {
        Console.Clear();
        Console.WriteLine("=== All Customers ===\n");

        var customers = db.Customers.ToList();

        if (customers.Count == 0)
        {
            Console.WriteLine("No customers found in the database. Returning to main menu...");
            return;
        }

        int fromTop = 2;

        foreach (var c in customers)
        {
            GUI.DrawWindow("Customer", 1, fromTop, new List<string> {
        $"ID:         {c.Id}",
        $"Name:       {c.FullName}",
        $"Email:      {c.Email}",
        $"Phone:      {c.Phone}",
        $"Address:    {c.Address}",
        $"PostalCode: {c.PostalCode}",
        $"City:       {c.City}",
        $"Country:    {c.Country}",
        $"Age:        {c.Age}",
        $"Username:   {c.Username ?? "Username not set"}",
        $"Role:       {c.Role.ToString()}"
    });

            fromTop += 13;
        }
    }


    public void AddCustomer()
    {
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

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nAccount successfully added to the database!");
        Console.ResetColor();
    }

    public void EditCustomer()
    {
        Console.Clear();
        Console.WriteLine("=== Edit Customer ===");

        var customer = CustomerSelector.ChooseCustomer(db);
        if (customer == null)
        {
            Console.WriteLine("No customer selected! Returning to main menu...");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine($"=== Editing customer: {customer.FullName} ===");

        string? newName = CustomerValidator.PromptOptionalName(customer.FullName!);
        if (!string.IsNullOrWhiteSpace(newName)) customer.FullName = newName;

        string? newEmail = CustomerValidator.PromptOptionalEmail(db, customer.Email!);
        if (!string.IsNullOrWhiteSpace(newEmail)) customer.Email = newEmail;

        string? newPhone = CustomerValidator.PromptOptionalPhone(customer.Phone!);
        if (!string.IsNullOrWhiteSpace(newPhone)) customer.Phone = newPhone;

        string? newAddress = CustomerValidator.PromptOptionalAddress(customer.Address!);
        if (!string.IsNullOrWhiteSpace(newAddress)) customer.Address = newAddress;

        string? newPostal = CustomerValidator.PromptOptionalPostalCode(customer.PostalCode!);
        if (!string.IsNullOrWhiteSpace(newPostal)) customer.PostalCode = newPostal;

        string? newCity = CustomerValidator.PromptOptionalCity(customer.City!);
        if (!string.IsNullOrWhiteSpace(newCity)) customer.City = newCity;

        string? newCountry = CustomerValidator.PromptOptionalCountry(customer.Country!);
        if (!string.IsNullOrWhiteSpace(newCountry)) customer.Country = newCountry;

        int? newAge = CustomerValidator.PromptOptionalAge(customer.Age!.Value);
        if (newAge.HasValue) customer.Age = newAge.Value;

        string? newUsername = CustomerValidator.PromptOptionalUsername(db, customer.Username!);
        if (!string.IsNullOrWhiteSpace(newUsername)) customer.Username = newUsername;

        string? newPassword = CustomerValidator.PromptOptionalPassword(customer.Password!);
        if (!string.IsNullOrWhiteSpace(newPassword)) customer.Password = newPassword;

        Role? newRole = CustomerValidator.PromptOptionalRole(customer.Role);
        if (newRole.HasValue) customer.Role = newRole.Value;


        db.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nCustomer updated successfully!");
        Console.ResetColor();
    }

    public void DeleteCustomer()
    {
        Console.Clear();
        Console.WriteLine("=== Delete a customer ===");

        var customer = CustomerSelector.ChooseCustomer(db);

        if (customer == null)
        {
            Console.WriteLine("Returning to main menu...");
            return;
        }

        Console.Clear();
        Console.WriteLine($"Are you sure you want to delete the user '{customer.FullName}'?");
        bool confirm = CustomerValidator.PromptConfirmation();

        if (confirm)
        {
            db.Customers.Remove(customer);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("User deleted successfully!");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Deletion cancelled! Returning to main menu...");
            Console.ResetColor();
        }
    }

}
