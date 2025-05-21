using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Menu
{
    public class AdminMenu
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;

        public AdminMenu(IProductService productService, ICustomerService customerService, ICategoryService categoryService, ISupplierService supplierService)
        {
            _productService = productService;
            _customerService = customerService;
            _categoryService = categoryService;
            _supplierService = supplierService;
        }

        public List<string> GetMenuItems()
        {
            return new List<string>
        {
                "1. Manage products",
                "2. Manage categories",
                "3. Manage customers",
                "4. Manage suppliers",
        };
        }

        public void ShowProductAdminMenu()
        {
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                GUI.DrawWindow("Product Menu", 0, 0, new List<string> {
                    "1. Show products",
                    "2. Add product",
                    "3. Edit product",
                    "4. Delete product",
                    "5. Return to Admin Menu"
                });

                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D5)
                {
                    inMenu = false;
                    continue;
                }

                bool isValidChoice = HandleProductInputs(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void ShowCategoryAdminMenu()
        {
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                GUI.DrawWindow("Category Administration", 0, 0, new List<string>{
                "1. Show categories",
                "2. Add category",
                "3. Edit category",
                "4. Delete category",
                "5. Return to Admin Menu"
            });

                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D5)
                {
                    inMenu = false;
                    continue;
                }

                bool isValidChoice = HandleCategoryInputs(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void ShowCustomerAdminMenu()
        {
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                GUI.DrawWindow("Customer Administration", 0, 0, new List<string>
{
                "1. Show customers",
                "2. Add customer",
                "3. Edit customer",
                "4. Delete customer",
                "5. Return to Admin Menu"
            });

                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D5)
                {
                    inMenu = false;
                    continue;
                }

                bool isValidChoice = HandleCustomerInputs(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }


        public void ShowSupplierAdminMenu()
        {
            bool inMenu = true;

            while (inMenu)
            {
                Console.Clear();
                GUI.DrawWindow("Supplier Administration", 0, 0, new List<string>
            {
                "1. Show all suppliers",
                "2. Add Supplier",
                "3. Edit Supplier",
                "4. Delete Supplier",
                "5. Return to Admin Menu"
            });

                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D5)
                {
                    inMenu = false;
                    continue;
                }

                bool isValidChoice = HandleSupplierInputs(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }


        private bool HandleProductInputs(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    _productService.ShowProducts(); return true;
                case ConsoleKey.D2:
                    _productService.AddProduct(); return true;
                case ConsoleKey.D3:
                    _productService.EditProduct(); return true;
                case ConsoleKey.D4:
                    _productService.DeleteProduct(); return true;
                default: return false;
            }
        }

        private bool HandleCategoryInputs(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    _categoryService.ShowAllCategories(); return true;
                case ConsoleKey.D2:
                    _categoryService.AddCategory(); return true;
                case ConsoleKey.D3:
                    _categoryService.EditCategory(); return true;
                case ConsoleKey.D4:
                    _categoryService.DeleteCategory(); return true;
                default: return false;
            }
        }

        private bool HandleCustomerInputs(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    _customerService.ShowAllCustomers(); return true;
                case ConsoleKey.D2:
                    _customerService.AddCustomer(); return true;
                case ConsoleKey.D3:
                    _customerService.EditCustomer(); return true;
                case ConsoleKey.D4:
                    _customerService.DeleteCustomer(); return true;
                default: return false;
            }
        }

        private bool HandleSupplierInputs(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    _supplierService.ShowAllSuppliers(); return true;
                case ConsoleKey.D2:
                    _supplierService.AddSupplier(); return true;
                case ConsoleKey.D3:
                    _supplierService.EditSupplier(); return true;
                case ConsoleKey.D4:
                    _supplierService.DeleteSupplier(); return true;
                default: return false;
            }
        }

    }
}