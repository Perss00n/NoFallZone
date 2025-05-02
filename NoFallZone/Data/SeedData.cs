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
            // Säkerställ att databasen är skapad
            context.Database.EnsureCreated();

            // Kategorier
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Klätterskor" },
                    new Category { Name = "Klätterselar" },
                    new Category { Name = "Klätterrep" },
                    new Category { Name = "Karbinhakar" },
                    new Category { Name = "Säkringsutrustning" }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Leverantörer
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

            // Produkter
            if (!context.Products.Any())
            {
                var categories = context.Categories.ToList();
                var suppliers = context.Suppliers.ToList();

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "La Sportiva Kataki",
                        Description = "Tekniska klätterskor för avancerad sportklättring.",
                        Price = 1899.00m,
                        Stock = 15,
                        IsFeatured = true,
                        CategoryId = categories.First(c => c.Name == "Klätterskor").Id,
                        SupplierId = suppliers.First(s => s.Name == "La Sportiva").Id
                    },
                    new Product
                    {
                        Name = "Black Diamond Momentum Harness",
                        Description = "Bekväm och justerbar klättersele för allroundbruk.",
                        Price = 799.00m,
                        Stock = 20,
                        IsFeatured = false,
                        CategoryId = categories.First(c => c.Name == "Klätterselar").Id,
                        SupplierId = suppliers.First(s => s.Name == "Black Diamond").Id
                    },
                    new Product
                    {
                        Name = "Petzl Arial 9.5mm 70m",
                        Description = "Lätt och hållbart klätterrep för sportklättring.",
                        Price = 2499.00m,
                        Stock = 10,
                        IsFeatured = true,
                        CategoryId = categories.First(c => c.Name == "Klätterrep").Id,
                        SupplierId = suppliers.First(s => s.Name == "Petzl").Id
                    },
                    new Product
                    {
                        Name = "Mammut Wall Alpine Belay",
                        Description = "Säkringsanordning för multipitch-klättring.",
                        Price = 599.00m,
                        Stock = 25,
                        IsFeatured = false,
                        CategoryId = categories.First(c => c.Name == "Säkringsutrustning").Id,
                        SupplierId = suppliers.First(s => s.Name == "Mammut").Id
                    },
                    new Product
                    {
                        Name = "Scarpa Drago",
                        Description = "Aggressiva klätterskor för bouldering och överhäng.",
                        Price = 1999.00m,
                        Stock = 12,
                        IsFeatured = true,
                        CategoryId = categories.First(c => c.Name == "Klätterskor").Id,
                        SupplierId = suppliers.First(s => s.Name == "Scarpa").Id
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            // Fraktalternativ
            if (!context.ShippingOptions.Any())
            {
                var shippingOptions = new List<ShippingOption>
                {
                    new ShippingOption { Name = "Standardfrakt (3-5 dagar)", Price = 59.00m },
                    new ShippingOption { Name = "Expressfrakt (1-2 dagar)", Price = 129.00m },
                    new ShippingOption { Name = "Gratis frakt vid köp över 1000 kr", Price = 0.00m }
                };
                context.ShippingOptions.AddRange(shippingOptions);
                context.SaveChanges();
            }

            // Kunder
            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer
                    {
                        FullName = "Anna Bergström",
                        Email = "anna.bergstrom@example.com",
                        Phone = "070-1234567",
                        Address = "Storgatan 1",
                        City = "Göteborg",
                        Country = "Sverige",
                        Age = 28
                    },
                    new Customer
                    {
                        FullName = "Erik Johansson",
                        Email = "erik.johansson@example.com",
                        Phone = "070-7654321",
                        Address = "Lillgatan 5",
                        City = "Stockholm",
                        Country = "Sverige",
                        Age = 35
                    }
                };
                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            // Orders och OrderItems
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
                    PaymentMethod = "Kort",
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
    }
}
