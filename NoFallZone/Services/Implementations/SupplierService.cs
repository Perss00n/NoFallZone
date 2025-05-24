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
            Console.WriteLine(DisplayHelper.ShowLogo());

            if (!db.Suppliers.Any())
            {
                GUI.DrawWindow("Suppliers", 1, 10, new List<string>
                {
                    "No suppliers found in the database."
                });
                return;
            }

            var suppliers = db.Suppliers.ToList();

            List<string> outputData = suppliers.Select(s => $"Id: {s.Id} | Name: {s.Name}").ToList();

            GUI.DrawWindow("Suppliers", 1, 10, outputData, 60);
        }

        public void AddSupplier()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine(DisplayHelper.ShowLogo());
            Console.WriteLine("=== Add a new supplier ===");

            string supplierName = SupplierValidator.PromptName();

            var newSupplier = new Supplier()
            {
                Name = supplierName
            };

            db.Suppliers.Add(newSupplier);

            if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
                OutputHelper.ShowSuccess("The Supplier has been added to the database!");
            else
                OutputHelper.ShowError(errorMsg);
        }

        public void EditSupplier()
        {
            if (!RequireAdminAccess()) return;

            var supplier = SupplierSelector.ChooseSupplier(db);

            if (supplier == null) return;

            string? newSupplierName = SupplierValidator.PromptOptionalName(supplier.Name!);
            if (!string.IsNullOrWhiteSpace(newSupplierName))
                supplier.Name = newSupplierName;

            if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
                OutputHelper.ShowSuccess("Supplier updated successfully!");
            else
                OutputHelper.ShowError(errorMsg);

        }

        public void DeleteSupplier()
        {
            if (!RequireAdminAccess()) return;

            Console.Clear();
            Console.WriteLine("=== Delete a supplier ===");

            var supplier = SupplierSelector.ChooseSupplier(db);
            if (supplier == null) return;

            Console.WriteLine($"Are you sure you want to delete '{supplier.Name}'?");
            if (!SupplierValidator.PromptConfirmation())
            {
                OutputHelper.ShowInfo("Cancelled.");
                return;
            }

            db.Suppliers.Remove(supplier);

            if (DatabaseHelper.TryToSaveToDb(db, out string errorMsg))
                OutputHelper.ShowSuccess("Supplier deleted successfully!");
            else
                OutputHelper.ShowError(errorMsg);
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