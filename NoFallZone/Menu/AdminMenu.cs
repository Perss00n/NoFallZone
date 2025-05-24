using NoFallZone.Services.Implementations;
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
        private readonly IShippingOptionService _shippingOptionService;
        private readonly IPaymentOptionService _paymentOptionService;

        public AdminMenu(IProductService productService, ICustomerService customerService, ICategoryService categoryService, ISupplierService supplierService, IShippingOptionService shippingOptionService, IPaymentOptionService paymentOptionService)
        {
            _productService = productService;
            _customerService = customerService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _shippingOptionService = shippingOptionService;
            _paymentOptionService = paymentOptionService;
        }

        public List<string> GetMenuItems()
        {
            return
        [
                "1. Manage products",
                "2. Manage categories",
                "3. Manage customers",
                "4. Manage suppliers",
                "5. Manage Shipping Options",
                "6. Manage Payment Options"
        ];
        }

        public void ShowProductAdminMenu()
        {
            bool inMenu = true;

            while (inMenu)
            {
                int fromLeft = 0;
                int fromTop = 10;
                string header = "Product Menu";
                List<string> lines = [
                    "1. Show products",
                    "2. Add product",
                    "3. Edit product",
                    "4. Delete product",
                    "5. Return to Admin Menu"
                ];
                Console.Clear();
                Console.WriteLine(DisplayHelper.ShowLogo());
                GUI.DrawWindow(header, fromLeft, fromTop, lines);

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
                int fromLeft = 0;
                int fromTop = 10;
                string header = "Category Menu";
                List<string> lines = [
                    "1. Show categories",
                    "2. Add category",
                    "3. Edit category",
                    "4. Delete category",
                    "5. Return to Admin Menu"
                ];

                Console.Clear();
                Console.WriteLine(DisplayHelper.ShowLogo());
                GUI.DrawWindow(header, fromLeft, fromTop, lines);

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
                int fromLeft = 0;
                int fromTop = 10;
                string header = "Customer Menu";
                List<string> lines = [
                    "1. Show customers",
                    "2. Add customer",
                    "3. Edit customer",
                    "4. Delete customer",
                    "5. Return to Admin Menu"
                 ];

                Console.Clear();
                Console.WriteLine(DisplayHelper.ShowLogo());
                GUI.DrawWindow(header, fromLeft, fromTop, lines);

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
                int fromLeft = 0;
                int fromTop = 10;
                string header = "Supplier Menu";
                List<string> lines = [
                    "1. Show all suppliers",
                    "2. Add supplier",
                    "3. Edit supplier",
                    "4. Delete supplier",
                    "5. Return to Admin Menu"
                ];

                Console.Clear();
                Console.WriteLine(DisplayHelper.ShowLogo());
                GUI.DrawWindow(header, fromLeft, fromTop, lines);

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

        public void ShowShippingOptionsAdminMenu()
        {
            bool inMenu = true;

            while (inMenu)
            {
                int fromLeft = 0;
                int fromTop = 10;
                string header = "Shipping Options Menu";
                List<string> lines = [
                    "1. Show all shipping options",
                    "2. Add new shipping option",
                    "3. Edit shipping option",
                    "4. Delete shipping option",
                    "5. Return to Admin Menu"
                ];

                Console.Clear();
                Console.WriteLine(DisplayHelper.ShowLogo());
                GUI.DrawWindow(header, fromLeft, fromTop, lines);

                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D5)
                {
                    inMenu = false;
                    continue;
                }

                bool isValidChoice = HandleShippingOptionsInputs(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public void ShowPaymentOptionsAdminMenu()
        {
            bool inMenu = true;

            while (inMenu)
            {
                int fromLeft = 0;
                int fromTop = 10;
                string header = "Payment Options Menu";
                List<string> lines = [
                    "1. Show all Payment options",
                    "2. Add new Payment option",
                    "3. Edit Payment option",
                    "4. Delete Payment option",
                    "5. Return to Admin Menu"
                ];

                Console.Clear();
                Console.WriteLine(DisplayHelper.ShowLogo());
                GUI.DrawWindow(header, fromLeft, fromTop, lines);

                var input = Console.ReadKey(true).Key;

                if (input == ConsoleKey.D5)
                {
                    inMenu = false;
                    continue;
                }

                bool isValidChoice = HandlePaymentOptionsInputs(input);

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

        private bool HandleShippingOptionsInputs(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    _shippingOptionService.ShowAllShippingOptions(); return true;
                case ConsoleKey.D2:
                    _shippingOptionService.AddShippingOption(); return true;
                case ConsoleKey.D3:
                    _shippingOptionService.EditShippingOption(); return true;
                case ConsoleKey.D4:
                    _shippingOptionService.DeleteShippingOption(); return true;
                default: return false;
            }
        }

        private bool HandlePaymentOptionsInputs(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    _paymentOptionService.ShowAllPaymentOptions(); return true;
                case ConsoleKey.D2:
                    _paymentOptionService.AddPaymentOption(); return true;
                case ConsoleKey.D3:
                    _paymentOptionService.EditPaymentOption(); return true;
                case ConsoleKey.D4:
                    _paymentOptionService.DeletePaymentOption(); return true;
                default: return false;
            }
        }

    }
}