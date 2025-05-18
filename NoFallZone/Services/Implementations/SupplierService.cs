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

        public void ShowAllSuppliers()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine("=== All Suppliers ===\n");

            var suppliers = db.Suppliers.ToList();

            if (suppliers.Count == 0)
            {
                GUI.DrawWindow("Suppliers", 1, 2, new List<string>
                {
                    "No suppliers found in the database."
                });
                return;
            }

            List<string> outputData = suppliers.Select(s => $"Id: {s.Id} | Name: {s.Name}").ToList();

            GUI.DrawWindow("Suppliers", 1, 2, outputData, maxLineWidth: 60);
        }

        public void AddSupplier()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine("=== Add a new supplier ===");

            string supplierName = SupplierValidator.PromptName();

            var newSupplier = new Supplier()
            {
                Name = supplierName
            };

            db.Suppliers.Add(newSupplier);
            db.SaveChanges();

            OutputHelper.ShowSuccess("The Supplier has been added to the database!");
        }

        public void EditSupplier()
        {
            if (!RequireAdminAccess()) return;

            var supplier = SupplierSelector.ChooseSupplier(db);

            if (supplier == null) return;

            string? newSupplierName = SupplierValidator.PromptOptionalName(supplier.Name!);
            if (!string.IsNullOrWhiteSpace(newSupplierName))
                supplier.Name = newSupplierName;

            db.SaveChanges();

            OutputHelper.ShowSuccess("Supplier updated successfully!");
        }

        public void DeleteSupplier()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine("=== Delete a supplier ===");

            var supplier = SupplierSelector.ChooseSupplier(db);
            if (supplier == null) return;

            Console.Clear();

            Console.WriteLine($"Are you sure you want to delete the supplier '{supplier.Name}'?");
            bool confirm = SupplierValidator.PromptConfirmation();

            if (confirm)
            {
                db.Suppliers.Remove(supplier);
                db.SaveChanges();

                OutputHelper.ShowSuccess("Supplier deleted successfully!");
            }
            else
            {
                OutputHelper.ShowError("Deletion cancelled! Returning to main menu...");
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
}