using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Models.Entities;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Selectors;
using NoFallZone.Utilities.SessionManagement;
using NoFallZone.Utilities.Validators;

namespace NoFallZone.Services.Implementations
{
    public class SupplierService : ISupplierService
    {
        private readonly NoFallZoneContext db;

        public SupplierService(NoFallZoneContext context)
        {
            db = context;
        }

        public async Task ShowAllSuppliersAsync()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());

            var suppliers = await db.Suppliers.ToListAsync();

            if (suppliers.Count == 0)
            {
                GUI.DrawWindow("Suppliers", 1, 10, new List<string>
        {
            "No suppliers found in the database."
        });
                return;
            }

            var outputData = suppliers.Select(s => $"Id: {s.Id} | Name: {s.Name}").ToList();
            GUI.DrawWindow("Suppliers", 1, 10, outputData, 60);
        }

        public async Task AddSupplierAsync()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            OutputHelper.ShowInfo("=== Add a new supplier ===");

            string supplierName = SupplierValidator.PromptName();

            var newSupplier = new Supplier()
            {
                Name = supplierName
            };

           await db.Suppliers.AddAsync(newSupplier);

            if (await DatabaseHelper.TryToSaveToDbAsync(db))
                OutputHelper.ShowSuccess("The Supplier has been added to the database!");

        }

        public async Task EditSupplierAsync()
        {
            if (!RequireAdminAccess()) return;

            var supplier = await SupplierSelector.ChooseSupplierAsync(db);

            if (supplier == null) return;

            string? newSupplierName = SupplierValidator.PromptOptionalName(supplier.Name!);
            if (!string.IsNullOrWhiteSpace(newSupplierName))
                supplier.Name = newSupplierName;

            if (await DatabaseHelper.TryToSaveToDbAsync(db))
                OutputHelper.ShowSuccess("Supplier updated successfully!");

        }

        public async Task DeleteSupplierAsync()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine("=== Delete a supplier ===");

            var supplier = await SupplierSelector.ChooseSupplierAsync(db);
            if (supplier == null) return;

            Console.WriteLine($"Are you sure you want to delete '{supplier.Name}'?");
            if (!SupplierValidator.PromptConfirmation())
            {
                OutputHelper.ShowInfo("Cancelled.");
                return;
            }

            db.Suppliers.Remove(supplier);

            if (await DatabaseHelper.TryToSaveToDbAsync(db))
                OutputHelper.ShowSuccess("Supplier deleted successfully!");
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
}