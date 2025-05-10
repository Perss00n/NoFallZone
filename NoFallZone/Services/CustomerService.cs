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


    public void AddCustomer()
    {
        Console.Clear();
        Console.WriteLine("=== Add a new customer ===");


        string name = InputHelper.PromptRequiredLimitedString("Enter your full name", 50, "Name can't be empty and can't exeed the maximum length! Please try again...");
        string email = InputHelper.PromptEmail("Enter your email", 70, "Invalid email! Please try again...");
        string phone = InputHelper.PromptPhone("Enter your phone number", 20, "Invalid phone number format! Please try again...");
        string adress = InputHelper.PromptRequiredLimitedString("Enter your adress", 100, "Adress can't be empty and can't exeed the maximum length! Please try again...");
        string postalCode = InputHelper.PromptPostalCode("Enter your postal code", 6, "Invalid postal code format! Please try again...");
        string city = InputHelper.PromptRequiredLimitedString("Enter your city", 50, "City can't be empty and can't exeed the maximum length! Please try again...");
        string country = InputHelper.PromptRequiredLimitedString("Enter your country", 55, "Country can't be empty and can't exeed the maximum length! Please try again...");
        int age = InputHelper.PromptInt("Enter your age", 1, 100, "Enter a valid age! Please try again...");

        var customer = new Customer
        {
            FullName = name,
            Email = email,
            Phone = phone,
            Address = adress,
            PostalCode = postalCode,
            City = city,
            Country = country,
            Age = age
        };

        db.Customers.Add(customer);
        db.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nAccount successfully added to the database!");
        Console.ResetColor();
    }

}
