# 🧗 NoFallZone – Climbing Gear Webshop (Console Edition)

**NoFallZone** is a full-featured climbing gear store simulation built with **C#**, using **Entity Framework Core** and **.NET Core**. It allows users to register, log in, browse and search for products, manage a cart, and place orders — all through a robust role-based system (User/Admin).  
This is a backend-focused project built as part of my backend development studies.

## 🔧 Tech Stack

- **.NET 8 / C#**
- **Entity Framework Core**
- **SQL Server**
- **Repository & Service Layer Pattern**
- **LINQ & Asynchronous Programming**
- **Clean Architecture Principles**

## 📦 Features

### 👤 User Functions:
- Register & login
- Browse products & view product details
- Search by keyword (name, description, supplier)
- View special deals
- Add products to cart
- Checkout and place orders

### 🛒 Cart System:
- View cart summary
- Quantity-based calculations
- Checkout with shipping & payment options

### 🔐 Admin Functions:
- Add/Edit/Delete: Products, Categories, Suppliers, Customers
- Manage shipping & payment options
- View statistics (top products, categories, etc.)

## 📁 Project Structure

```plaintext
NoFallZone/
├── Data/                  # EF Core DB context and seed logic
├── Menu/                  # Console menu logic (Admin/Customer)
├── Migrations/            # EF Core migrations history
├── Models/
│   ├── Entities/          # Domain models (Customer, Product, Order, etc.)
│   └── Enums/             # Enum definitions (e.g. Role)
├── Services/
│   ├── Implementations/   # Business logic implementations
│   └── Interfaces/        # Service interfaces for DI
├── Setup/                 # Startup and service configuration
├── Utilities/
│   ├── Helpers/           # Utility classes (Display, Input, Login, Output, etc.)
│   ├── Selectors/         # Selectors for choosing entities via console
│   ├── SessionManagement/ # Session handling for user and cart
│   └── Validators/        # Input validation for models
├── appsettings.json       # Config file including DB connection
└── Program.cs             # Entry point
```

## 👨‍💻 About the Author

**Marcus Lehm**

A backend development student at Uddevalla Yrkeshögskola (Sweden) with a background in flooring and a new career in code.
This project is part of my learning journey into professional backend development.

📬 Reach me at:

Perss00n@gmail.com

<a href="https://marcuslehm.se" target="_blank" rel="noopener noreferrer">marcuslehm.se</a>

