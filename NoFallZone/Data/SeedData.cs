using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Models;

namespace NoFallZone.Utilities
{
    public static class SeedData
    {
        public static void Initialize(NoFallZoneContext context)
        {
            context.Database.EnsureCreated();

            // Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Climbing Shoes" },
                    new Category { Name = "Harnesses" },
                    new Category { Name = "Ropes" },
                    new Category { Name = "Carabiners" },
                    new Category { Name = "Belay Devices" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Suppliers
            if (!context.Suppliers.Any())
            {
                var suppliers = new List<Supplier>
                {
                    new Supplier { Name = "La Sportiva" },
                    new Supplier { Name = "Black Diamond" },
                    new Supplier { Name = "Petzl" },
                    new Supplier { Name = "Mammut" },
                    new Supplier { Name = "Scarpa" }
                };
                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }

            // Products
            if (!context.Products.Any())
            {
                var categories = context.Categories.ToList();
                var suppliers = context.Suppliers.ToList();

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "La Sportiva Kataki",
                        Description = "Technical climbing shoes designed for advanced sport climbing.",
                        Price = 1899.00m,
                        Stock = 15,
                        IsFeatured = true,
                        CategoryId = categories.First(c => c.Name == "Climbing Shoes").Id,
                        SupplierId = suppliers.First(s => s.Name == "La Sportiva").Id
                    },
                    new Product
                    {
                        Name = "Black Diamond Momentum Harness",
                        Description = "Comfortable and adjustable all-around climbing harness.",
                        Price = 799.00m,
                        Stock = 20,
                        IsFeatured = false,
                        CategoryId = categories.First(c => c.Name == "Harnesses").Id,
                        SupplierId = suppliers.First(s => s.Name == "Black Diamond").Id
                    },
                    new Product
                    {
                        Name = "Petzl Arial 9.5mm 70m",
                        Description = "Light and durable climbing rope for sport climbing.",
                        Price = 2499.00m,
                        Stock = 10,
                        IsFeatured = true,
                        CategoryId = categories.First(c => c.Name == "Ropes").Id,
                        SupplierId = suppliers.First(s => s.Name == "Petzl").Id
                    },
                    new Product
                    {
                        Name = "Mammut Wall Alpine Belay",
                        Description = "Belay device designed for multipitch climbing and rappelling.",
                        Price = 599.00m,
                        Stock = 25,
                        IsFeatured = false,
                        CategoryId = categories.First(c => c.Name == "Belay Devices").Id,
                        SupplierId = suppliers.First(s => s.Name == "Mammut").Id
                    },
                    new Product
                    {
                        Name = "Scarpa Drago",
                        Description = "Aggressive bouldering shoes with sensitive sole and tight fit.",
                        Price = 1999.00m,
                        Stock = 12,
                        IsFeatured = true,
                        CategoryId = categories.First(c => c.Name == "Climbing Shoes").Id,
                        SupplierId = suppliers.First(s => s.Name == "Scarpa").Id
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // Shipping Options
            if (!context.ShippingOptions.Any())
            {
                var shippingOptions = new List<ShippingOption>
                {
                    new ShippingOption { Name = "Standard Shipping (3–5 days)", Price = 59.00m },
                    new ShippingOption { Name = "Express Shipping (1–2 days)", Price = 129.00m },
                    new ShippingOption { Name = "Free Shipping on Orders over 1000 SEK", Price = 0.00m }
                };
                context.ShippingOptions.AddRange(shippingOptions);
                context.SaveChanges();
            }

            // Customers
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer
                    {
                        FullName = "Anna Bergstrom",
                        Email = "anna.bergstrom@example.com",
                        Phone = "070-1234567",
                        Address = "Storgatan 1",
                        PostalCode = "451 34",
                        City = "Gothenburg",
                        Country = "Sweden",
                        Age = 28
                    },
                    new Customer
                    {
                        FullName = "Erik Johansson",
                        Email = "erik.johansson@example.com",
                        Phone = "070-7654321",
                        Address = "Lillgatan 5",
                        PostalCode = "451 54",
                        City = "Stockholm",
                        Country = "Sweden",
                        Age = 35
                    }
                };
                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            // Orders + OrderItems
            if (!context.Orders.Any())
            {
                var customers = context.Customers.ToList();
                var shippingOptions = context.ShippingOptions.ToList();
                var products = context.Products.ToList();

                var order1 = new Order
                {
                    OrderDate = DateTime.Now.AddDays(-10),
                    TotalPrice = 1999.00m + 59.00m,
                    ShippingCost = 59.00m,
                    PaymentMethod = "Credit Card",
                    CustomerId = customers.First().Id,
                    ShippingOptionId = shippingOptions.First().Id
                };

                var order2 = new Order
                {
                    OrderDate = DateTime.Now.AddDays(-5),
                    TotalPrice = 799.00m + 129.00m,
                    ShippingCost = 129.00m,
                    PaymentMethod = "Swish",
                    CustomerId = customers.Last().Id,
                    ShippingOptionId = shippingOptions[1].Id
                };

                context.Orders.AddRange(order1, order2);
                context.SaveChanges();

                var orderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        OrderId = order1.Id,
                        ProductId = products.First(p => p.Name == "Scarpa Drago").Id,
                        Quantity = 1,
                        PricePerUnit = 1999.00m
                    },
                    new OrderItem
                    {
                        OrderId = order2.Id,
                        ProductId = products.First(p => p.Name == "Black Diamond Momentum Harness").Id,
                        Quantity = 1,
                        PricePerUnit = 799.00m
                    }
                };

                context.OrderItems.AddRange(orderItems);
                context.SaveChanges();
            }
        }


        public static void ClearDatabase(NoFallZoneContext context)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: This will delete all data in the database.");
            Console.WriteLine("Are you sure you want to continue?");
            Console.ResetColor();

            if (!InputHelper.PromptYesNo("Confirm wipe", "Please select 'Y' for Yes and 'N' for No")) return;

            context.OrderItems.RemoveRange(context.OrderItems);
            context.Orders.RemoveRange(context.Orders);
            context.Customers.RemoveRange(context.Customers);
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);
            context.Suppliers.RemoveRange(context.Suppliers);
            context.ShippingOptions.RemoveRange(context.ShippingOptions);

            context.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("All data has been cleared.");
            Console.ResetColor();
        }


    }
}
