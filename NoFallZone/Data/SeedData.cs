using NoFallZone.Models.Entities;
using NoFallZone.Models.Enums;
using NoFallZone.Utilities.Helpers;
namespace NoFallZone.Data;
public static class SeedData
{
    public static async Task InitializeAsync(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());

        if (!InputHelper.PromptYesNo("\nAre you sure you want to populate the DB with info?", "Please enter 'Y' for yes and 'N' for no!"))
        {
            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            OutputHelper.ShowInfo("The action was cancelled!");
            return;
        }
        var categories = new List<Category>
        {
            new Category { Name = "Climbing Shoes" },
            new Category { Name = "Harnesses" },
            new Category { Name = "Ropes" },
            new Category { Name = "Carabiners" },
            new Category { Name = "Belay Devices" },
            new Category { Name = "Climbing Helmets" },
            new Category { Name = "Quick Draws" },
            new Category { Name = "Slings & Runners" }

        };
        db.Categories.AddRange(categories);
        await DatabaseHelper.TryToSaveToDbAsync(db);



        var suppliers = new List<Supplier>
        {
            new Supplier { Name = "Petzl" },
            new Supplier { Name = "Black Diamond" },
            new Supplier { Name = "Mammut" },
            new Supplier { Name = "La Sportiva" },
            new Supplier { Name = "DMM" },
            new Supplier { Name = "Edelrid" },
            new Supplier { Name = "Wild Country" },
            new Supplier { Name = "Blue Ice" }
        };
        db.Suppliers.AddRange(suppliers);
        await DatabaseHelper.TryToSaveToDbAsync(db);


        var products = new List<Product>
        {
            new Product { Name = "La Sportiva Solution", Description = "Aggressive shoe ideal for overhanging routes.", Price = 2404, Stock = 12, CategoryId = categories[0].Id, SupplierId = suppliers[3].Id, IsFeatured = true },
            new Product { Name = "La Sportiva Miura", Description = "Precision shoe for vertical and overhanging routes.", Price = 1542, Stock = 10, CategoryId = categories[0].Id, SupplierId = suppliers[3].Id, IsFeatured = false },
            new Product { Name = "La Sportiva Tarantula", Description = "Comfortable and durable entry-level shoe.", Price = 1122, Stock = 7, CategoryId = categories[0].Id, SupplierId = suppliers[3].Id, IsFeatured = false },
            new Product { Name = "La Sportiva Finale", Description = "Great for beginners and long climbs.", Price = 999, Stock = 8, CategoryId = categories[0].Id, SupplierId = suppliers[3].Id, IsFeatured = false },
            new Product { Name = "La Sportiva Katana", Description = "All-around climbing shoe with great edging.", Price = 1650, Stock = 5, CategoryId = categories[0].Id, SupplierId = suppliers[3].Id, IsFeatured = false },

            new Product { Name = "Black Diamond Solution", Description = "Comfortable sport climbing harness.", Price = 2200, Stock = 13, CategoryId = categories[1].Id, SupplierId = suppliers[1].Id, IsFeatured = false },
            new Product { Name = "Petzl Adjama", Description = "Versatile harness for mountaineering and trad.", Price = 1470, Stock = 2, CategoryId = categories[1].Id, SupplierId = suppliers[0].Id, IsFeatured = false },
            new Product { Name = "Mammut Ophir 3 Slide", Description = "Adjustable and breathable for all-round use.", Price = 888, Stock = 14, CategoryId = categories[1].Id, SupplierId = suppliers[2].Id, IsFeatured = false },
            new Product { Name = "Black Diamond Momentum", Description = "All-around harness for any climbing style.", Price = 1175, Stock = 9, CategoryId = categories[1].Id, SupplierId = suppliers[1].Id, IsFeatured = false },
            new Product { Name = "Petzl Sama", Description = "Sport climbing harness with breathable design.", Price = 1250, Stock = 6, CategoryId = categories[1].Id, SupplierId = suppliers[0].Id, IsFeatured = false },

            new Product { Name = "Petzl Arial 9.5mm", Description = "Durable climbing rope for sport and trad.", Price = 2019, Stock = 11, CategoryId = categories[2].Id, SupplierId = suppliers[0].Id, IsFeatured = false },
            new Product { Name = "Mammut Infinity Dry 9.5mm", Description = "Dry-treated rope for alpine routes.", Price = 1773, Stock = 3, CategoryId = categories[2].Id, SupplierId = suppliers[2].Id, IsFeatured = false },
            new Product { Name = "Edelrid Boa Eco 9.8mm", Description = "Eco-friendly rope made from leftover yarn.", Price = 606, Stock = 10, CategoryId = categories[2].Id, SupplierId = suppliers[5].Id, IsFeatured = true },
            new Product { Name = "Edelrid Eagle Lite Eco Dry 9.5mm", Description = "Robust dry-treated rope for sport and trad.", Price = 1850, Stock = 4, CategoryId = categories[2].Id, SupplierId = suppliers[5].Id, IsFeatured = false },
            new Product { Name = "Mammut 9.8 Crag Classic", Description = "Durable rope for all-around climbing.", Price = 1399, Stock = 8, CategoryId = categories[2].Id, SupplierId = suppliers[2].Id, IsFeatured = false },

            new Product { Name = "DMM Phantom", Description = "Ultralight locking carabiner.", Price = 998, Stock = 4, CategoryId = categories[3].Id, SupplierId = suppliers[4].Id, IsFeatured = false },
            new Product { Name = "Petzl Spirit", Description = "Lightweight solid gate carabiner.", Price = 1038, Stock = 2, CategoryId = categories[3].Id, SupplierId = suppliers[0].Id, IsFeatured = false },
            new Product { Name = "Wild Country Helium 3.0", Description = "Light and strong for trad.", Price = 1211, Stock = 12, CategoryId = categories[3].Id, SupplierId = suppliers[6].Id, IsFeatured = false },
            new Product { Name = "Edelrid Bulletproof", Description = "Durable steel insert to reduce wear.", Price = 2320, Stock = 8, CategoryId = categories[3].Id, SupplierId = suppliers[5].Id, IsFeatured = false },
            new Product { Name = "Petzl Am D", Description = "Asymmetric aluminum carabiner for belay systems.", Price = 890, Stock = 6, CategoryId = categories[3].Id, SupplierId = suppliers[0].Id, IsFeatured = false },

            new Product { Name = "Petzl Grigri+", Description = "Assisted braking device for sport and gym.", Price = 1632, Stock = 2, CategoryId = categories[4].Id, SupplierId = suppliers[0].Id, IsFeatured = false },
            new Product { Name = "DMM Pivot", Description = "Smooth lowering even under load.", Price = 2083, Stock = 11, CategoryId = categories[4].Id, SupplierId = suppliers[4].Id, IsFeatured = false },
            new Product { Name = "Mammut Smart 2.0", Description = "Intuitive assisted braking.", Price = 2464, Stock = 9, CategoryId = categories[4].Id, SupplierId = suppliers[2].Id, IsFeatured = false },
            new Product { Name = "Wild Country Revo", Description = "Unique bi-directional assisted braking.", Price = 2026, Stock = 14, CategoryId = categories[4].Id, SupplierId = suppliers[6].Id, IsFeatured = false },
            new Product { Name = "Edelrid Mega Jul", Description = "Compact assisted braking belay device.", Price = 990, Stock = 7, CategoryId = categories[4].Id, SupplierId = suppliers[5].Id, IsFeatured = false },

            new Product { Name = "Black Diamond Vapor", Description = "Ultralight helmet for alpine missions.", Price = 1070, Stock = 11, CategoryId = categories[5].Id, SupplierId = suppliers[1].Id, IsFeatured = false },
            new Product { Name = "Petzl Meteor", Description = "All-purpose helmet with great ventilation.", Price = 2152, Stock = 5, CategoryId = categories[5].Id, SupplierId = suppliers[0].Id, IsFeatured = false },
            new Product { Name = "Mammut Wall Rider", Description = "Lightweight with EPP foam.", Price = 1602, Stock = 13, CategoryId = categories[5].Id, SupplierId = suppliers[2].Id, IsFeatured = false },
            new Product { Name = "Black Diamond Half Dome", Description = "Durable helmet with improved fit and comfort.", Price = 799, Stock = 4, CategoryId = categories[5].Id, SupplierId = suppliers[1].Id, IsFeatured = false },
            new Product { Name = "Petzl Boreo", Description = "Versatile and durable helmet.", Price = 999, Stock = 6, CategoryId = categories[5].Id, SupplierId = suppliers[0].Id, IsFeatured = false },

            new Product { Name = "Black Diamond Positron", Description = "Solid all-round quickdraw.", Price = 1254, Stock = 12, CategoryId = categories[6].Id, SupplierId = suppliers[1].Id, IsFeatured = true },
            new Product { Name = "Petzl Djinn Axess", Description = "Durable for beginners and pros.", Price = 1890, Stock = 12, CategoryId = categories[6].Id, SupplierId = suppliers[0].Id, IsFeatured = false },
            new Product { Name = "Wild Country Proton", Description = "Great clipping action.", Price = 2044, Stock = 3, CategoryId = categories[6].Id, SupplierId = suppliers[6].Id, IsFeatured = false },
            new Product { Name = "DMM Alpha Sport", Description = "Ergonomic and robust design.", Price = 2084, Stock = 13, CategoryId = categories[6].Id, SupplierId = suppliers[4].Id, IsFeatured = false },
            new Product { Name = "Wild Country Session", Description = "Versatile and ergonomic quickdraw.", Price = 1432, Stock = 6, CategoryId = categories[6].Id, SupplierId = suppliers[6].Id, IsFeatured = false },

            new Product { Name = "Blue Ice Alpine Runner", Description = "Strong and lightweight runner.", Price = 2123, Stock = 7, CategoryId = categories[7].Id, SupplierId = suppliers[7].Id, IsFeatured = false },
            new Product { Name = "Edelrid Tech Web Sling", Description = "Combines strength and flexibility.", Price = 1390, Stock = 8, CategoryId = categories[7].Id, SupplierId = suppliers[5].Id, IsFeatured = false },
            new Product { Name = "DMM Dynatec Sling", Description = "Light and abrasion-resistant.", Price = 1530, Stock = 2, CategoryId = categories[7].Id, SupplierId = suppliers[4].Id, IsFeatured = false },
            new Product { Name = "Edelrid Aramid Sling", Description = "Flexible and heat-resistant.", Price = 1458, Stock = 11, CategoryId = categories[7].Id, SupplierId = suppliers[5].Id, IsFeatured = false },
            new Product { Name = "Petzl St’Anneau", Description = "Great for alpine anchors and extensions.", Price = 1403, Stock = 10, CategoryId = categories[7].Id, SupplierId = suppliers[0].Id, IsFeatured = false }
        };

        db.Products.AddRange(products);
        await DatabaseHelper.TryToSaveToDbAsync(db);


        var shippingOptions = new List<ShippingOption>
        {
            new ShippingOption { Name = "Standard (3–5 business days)", Price = 59 },
            new ShippingOption { Name = "Express (1–2 business days)", Price = 99 },
            new ShippingOption { Name = "Next Day Delivery (order before 14:00)", Price = 149 },
            new ShippingOption { Name = "Pickup Point (2–4 business days)", Price = 39 },
            new ShippingOption { Name = "Home Delivery (4-5 business days)", Price = 149 },
            new ShippingOption { Name = "Free Shipping (5–7 business days, orders over 1000 kr)", Price = 0 }
        };
        db.ShippingOptions.AddRange(shippingOptions);
        await DatabaseHelper.TryToSaveToDbAsync(db);



        var paymentOptions = new List<PaymentOption>
        {
            new PaymentOption { Name = "Credit/Debit Card (Visa, MasterCard)", Fee = 10 },
            new PaymentOption { Name = "Swish (Mobile payment via BankID)", Fee = 0 },
            new PaymentOption { Name = "PayPal", Fee = 29 },
            new PaymentOption { Name = "Invoice (14 days payment, via Klarna)", Fee = 19 },
            new PaymentOption { Name = "Direct Bank Transfer (Instant checkout)", Fee = 0 }
        };
        db.PaymentOptions.AddRange(paymentOptions);
        await DatabaseHelper.TryToSaveToDbAsync(db);



        var customers = new List<Customer>
    {
        new Customer
        {
            FullName = "Marcus Lehm",
            Email = "Perss00n@gmail.com",
            Phone = "073-7871226",
            Address = "Äsperödsvägen 10B",
            PostalCode = "451 34",
            City = "Uddevalla",
            Country = "Sweden",
            Age = 38,
            Username = "Perss00n",
            Password = "thisisatest",
            Role = Role.Admin
        },
        new Customer
        {
            FullName = "Emma Johansson",
            Email = "emma.johansson@example.com",
            Phone = "070-1234567",
            Address = "Storgatan 12",
            PostalCode = "111 22",
            City = "Stockholm",
            Country = "Sweden",
            Age = 29,
            Username = "emmaj",
            Password = "securepass1",
            Role = Role.User
        },
        new Customer
        {
            FullName = "Liam Nilsson",
            Email = "liam.nilsson@example.com",
            Phone = "073-9876543",
            Address = "Bergsvägen 5",
            PostalCode = "222 33",
            City = "Göteborg",
            Country = "Sweden",
            Age = 35,
            Username = "liamn",
            Password = "securepass2",
            Role = Role.User
        },
        new Customer
        {
            FullName = "Alice Eriksson",
            Email = "alice.eriksson@example.com",
            Phone = "076-3214567",
            Address = "Havsutsiktsvägen 7",
            PostalCode = "333 44",
            City = "Malmö",
            Country = "Sweden",
            Age = 31,
            Username = "alicee",
            Password = "securepass3",
            Role = Role.User
        },
        new Customer
        {
            FullName = "Noah Svensson",
            Email = "noah.svensson@example.com",
            Phone = "070-6543210",
            Address = "Tallstigen 9",
            PostalCode = "444 55",
            City = "Uppsala",
            Country = "Sweden",
            Age = 27,
            Username = "noahs",
            Password = "securepass4",
            Role = Role.User
        },
        new Customer
        {
            FullName = "Olivia Andersson",
            Email = "olivia.andersson@example.com",
            Phone = "072-1112233",
            Address = "Fjällgatan 3",
            PostalCode = "555 66",
            City = "Kiruna",
            Country = "Sweden",
            Age = 34,
            Username = "oliviaa",
            Password = "securepass5",
            Role = Role.User
        },
        new Customer
        {
            FullName = "William Lindberg",
            Email = "william.lindberg@example.com",
            Phone = "073-2323232",
            Address = "Lindgrens väg 22",
            PostalCode = "666 77",
            City = "Karlstad",
            Country = "Sweden",
            Age = 30,
            Username = "williaml",
            Password = "securepass6",
            Role = Role.User
        },
        new Customer
        {
            FullName = "Ebba Holm",
            Email = "ebba.holm@example.com",
            Phone = "075-5566778",
            Address = "Skogsvägen 1",
            PostalCode = "777 88",
            City = "Örebro",
            Country = "Sweden",
            Age = 26,
            Username = "ebbah",
            Password = "securepass7",
            Role = Role.User
        },
        new Customer
        {
            FullName = "Lucas Bergström",
            Email = "lucas.bergstrom@example.com",
            Phone = "079-4455667",
            Address = "Kustgatan 19",
            PostalCode = "888 99",
            City = "Sundsvall",
            Country = "Sweden",
            Age = 32,
            Username = "lucasb",
            Password = "securepass8",
            Role = Role.User
        },
        new Customer
        {
            FullName = "Elsa Norberg",
            Email = "elsa.norberg@example.com",
            Phone = "076-8889990",
            Address = "Ängsvägen 15",
            PostalCode = "999 00",
            City = "Halmstad",
            Country = "Sweden",
            Age = 28,
            Username = "elsan",
            Password = "securepass9",
            Role = Role.User
        }
    };
        db.Customers.AddRange(customers);
        await DatabaseHelper.TryToSaveToDbAsync(db);


        var orders = new List<Order>
        {
            new Order
            {
                CustomerId = customers[1].Id,
                OrderDate = DateTime.Now,
                ShippingOptionId = shippingOptions[2].Id,
                ShippingCost = shippingOptions[2].Price,
                PaymentOptionId = paymentOptions[3].Id,
                PaymentOptionName = paymentOptions[3].Name,
                TotalPrice = 11433,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = products[33].Id, Quantity = 1, PricePerUnit = 1457, WasDeal = true },
                    new OrderItem { ProductId = products[35].Id, Quantity = 2, PricePerUnit = 1448, WasDeal = false },
                    new OrderItem { ProductId = products[2].Id, Quantity = 3, PricePerUnit = 2304, WasDeal = true }
                }
            },
            new Order
            {
                CustomerId = customers[2].Id,
                OrderDate = DateTime.Now,
                ShippingOptionId = shippingOptions[1].Id,
                ShippingCost = shippingOptions[1].Price,
                PaymentOptionId = paymentOptions[0].Id,
                PaymentOptionName = paymentOptions[0].Name,
                TotalPrice = 8676,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = products[26].Id, Quantity = 3, PricePerUnit = 891, WasDeal = false},
                    new OrderItem { ProductId = products[8].Id, Quantity = 2, PricePerUnit = 1243, WasDeal = false },
                    new OrderItem { ProductId = products[37].Id, Quantity = 3, PricePerUnit = 1136, WasDeal = true }
                }
            },
            new Order
            {
                CustomerId = customers[3].Id,
                OrderDate = DateTime.Now,
                ShippingOptionId = shippingOptions[0].Id,
                ShippingCost = shippingOptions[0].Price,
                PaymentOptionId = paymentOptions[0].Id,
                PaymentOptionName = paymentOptions[0].Name,
                TotalPrice = 9807,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = products[0].Id, Quantity = 3, PricePerUnit = 1772, WasDeal = true },
                    new OrderItem { ProductId = products[27].Id, Quantity = 2, PricePerUnit = 2211, WasDeal = true }
                }
            },
            new Order
            {
                CustomerId = customers[4].Id,
                OrderDate = DateTime.Now,
                ShippingOptionId = shippingOptions[0].Id,
                ShippingCost = shippingOptions[0].Price,
                PaymentOptionId = paymentOptions[2].Id,
                PaymentOptionName = paymentOptions[2].Name,
                TotalPrice = 6622,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = products[1].Id, Quantity = 2, PricePerUnit = 1526, WasDeal = false },
                    new OrderItem { ProductId = products[12].Id, Quantity = 1, PricePerUnit = 1402, WasDeal = false },
                    new OrderItem { ProductId = products[17].Id, Quantity = 1, PricePerUnit = 2080, WasDeal = false }
                }
            },
            new Order
            {
                CustomerId = customers[0].Id,
                OrderDate = DateTime.Now,
                ShippingOptionId = shippingOptions[2].Id,
                ShippingCost = shippingOptions[2].Price,
                PaymentOptionId = paymentOptions[1].Id,
                PaymentOptionName = paymentOptions[1].Name,
                TotalPrice = 7404,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = products[19].Id, Quantity = 2, PricePerUnit = 1900, WasDeal = true },
                    new OrderItem { ProductId = products[6].Id, Quantity = 1, PricePerUnit = 1555, WasDeal = false },
                    new OrderItem { ProductId = products[23].Id, Quantity = 1, PricePerUnit = 1990, WasDeal = true }
                }
            }
        };

        db.Orders.AddRange(orders);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The database has successfully been filled with some example data!");
    }


    public static async Task ClearDatabaseAsync(NoFallZoneContext db)
    {
        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        if (!InputHelper.PromptYesNo("\nAre you sure you want to clear the database?", "Please enter 'Y' for yes and 'N' for no!"))
        {
            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            OutputHelper.ShowInfo("The action was cancelled!");
            return;
        }

        db.OrderItems.RemoveRange(db.OrderItems);
        db.Orders.RemoveRange(db.Orders);

        db.Customers.RemoveRange(db.Customers);

        db.Products.RemoveRange(db.Products);
        db.Categories.RemoveRange(db.Categories);
        db.Suppliers.RemoveRange(db.Suppliers);

        db.ShippingOptions.RemoveRange(db.ShippingOptions);
        db.PaymentOptions.RemoveRange(db.PaymentOptions);
        db.ActivityLogs.RemoveRange(db.ActivityLogs);

        if (await DatabaseHelper.TryToSaveToDbAsync(db))
            OutputHelper.ShowSuccess("The database was successfully cleared!");
    }

}