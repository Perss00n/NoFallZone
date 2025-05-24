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

        public async Task ShowProductAdminMenuAsync()
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

                bool isValidChoice = await HandleProductInputsAsync(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public async Task ShowCategoryAdminMenuAsync()
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

                bool isValidChoice = await HandleCategoryInputsAsync(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public async Task ShowCustomerAdminMenuAsync()
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

                bool isValidChoice = await HandleCustomerInputsAsync(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public async Task ShowSupplierAdminMenuAsync()
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

                bool isValidChoice = await HandleSupplierInputsAsync(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public async Task ShowShippingOptionsAdminMenuAsync()
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

                bool isValidChoice = await HandleShippingOptionsInputsAsync(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }

        public async Task ShowPaymentOptionsAdminMenuAsync()
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

                bool isValidChoice = await HandlePaymentOptionsInputsAsync(input);

                if (!isValidChoice)
                {
                    Console.Clear();
                    OutputHelper.ShowError("Invalid choice!");
                }

                OutputHelper.ShowInfo("Press any key to continue...");
                Console.ReadKey();
            }
        }


        private async Task<bool> HandleProductInputsAsync(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                   await _productService.ShowProductsAsync(); return true;
                case ConsoleKey.D2:
                    await _productService.AddProductAsync(); return true;
                case ConsoleKey.D3:
                    await _productService.EditProductAsync(); return true;
                case ConsoleKey.D4:
                    await _productService.DeleteProductAsync(); return true;
                default: return false;
            }
        }

        private async Task<bool> HandleCategoryInputsAsync(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    await _categoryService.ShowAllCategoriesAsync(); return true;
                case ConsoleKey.D2:
                    await _categoryService.AddCategoryAsync(); return true;
                case ConsoleKey.D3:
                    await _categoryService.EditCategoryAsync(); return true;
                case ConsoleKey.D4:
                    await _categoryService.DeleteCategoryAsync(); return true;
                default: return false;
            }
        }

        private async Task<bool> HandleCustomerInputsAsync(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    await _customerService.ShowAllCustomersAsync(); return true;
                case ConsoleKey.D2:
                    await _customerService.AddCustomerAsync(); return true;
                case ConsoleKey.D3:
                    await _customerService.EditCustomerAsync(); return true;
                case ConsoleKey.D4:
                    await _customerService.DeleteCustomerAsync(); return true;
                default: return false;
            }
        }

        private async Task<bool> HandleSupplierInputsAsync(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    await _supplierService.ShowAllSuppliersAsync(); return true;
                case ConsoleKey.D2:
                    await _supplierService.AddSupplierAsync(); return true;
                case ConsoleKey.D3:
                    await _supplierService.EditSupplierAsync(); return true;
                case ConsoleKey.D4:
                    await _supplierService.DeleteSupplierAsync(); return true;
                default: return false;
            }
        }

        private async Task<bool> HandleShippingOptionsInputsAsync(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    await _shippingOptionService.ShowAllShippingOptionsAsync(); return true;
                case ConsoleKey.D2:
                    await _shippingOptionService.AddShippingOptionAsync(); return true;
                case ConsoleKey.D3:
                    await _shippingOptionService.EditShippingOptionAsync(); return true;
                case ConsoleKey.D4:
                    await _shippingOptionService.DeleteShippingOptionAsync(); return true;
                default: return false;
            }
        }

        private async Task<bool> HandlePaymentOptionsInputsAsync(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    await _paymentOptionService.ShowAllPaymentOptionsAsync(); return true;
                case ConsoleKey.D2:
                    await _paymentOptionService.AddPaymentOptionAsync(); return true;
                case ConsoleKey.D3:
                    await _paymentOptionService.EditPaymentOptionAsync(); return true;
                case ConsoleKey.D4:
                    await _paymentOptionService.DeletePaymentOptionAsync(); return true;
                default: return false;
            }
        }

    }
}