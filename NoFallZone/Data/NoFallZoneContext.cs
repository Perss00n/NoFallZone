using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NoFallZone.Models.Entities;

namespace NoFallZone.Data;
public class NoFallZoneContext : DbContext
{


    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ShippingOption> ShippingOptions { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnectionString")!;
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Product
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .IsRequired();
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .IsRequired()
            .HasPrecision(18, 2);
        modelBuilder.Entity<Product>()
            .Property(p => p.Stock)
            .IsRequired();
        modelBuilder.Entity<Product>()
            .Property(p => p.IsFeatured)
            .HasDefaultValue(false);
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name)
            .IsUnique();

        // Order
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Order>()
            .Property(o => o.ShippingCost)
            .HasPrecision(18, 2);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.ShippingOption)
            .WithMany(so => so.Orders)
            .HasForeignKey(o => o.ShippingOptionId)
            .OnDelete(DeleteBehavior.SetNull);

        // OrderItem
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.PricePerUnit)
            .IsRequired()
            .HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.Quantity)
            .IsRequired();
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Customer
        modelBuilder.Entity<Customer>()
            .Property(c => c.FullName)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Email)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Username)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Password)
            .IsRequired();
        modelBuilder.Entity<Customer>()
            .Property(c => c.Role)
            .HasConversion<string>();
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Username)
            .IsUnique();

        // ShippingOption
        modelBuilder.Entity<ShippingOption>()
            .Property(so => so.Price)
            .IsRequired()
            .HasPrecision(18, 2);
        modelBuilder.Entity<ShippingOption>()
            .Property(so => so.Name)
            .IsRequired();

        // Category
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired();
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        // Supplier
        modelBuilder.Entity<Supplier>()
            .Property(s => s.Name)
            .IsRequired();
        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.Name)
            .IsUnique();
    }


}
