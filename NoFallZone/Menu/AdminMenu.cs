using NoFallZone.Services;
using NoFallZone.Utilities;

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
            Console.Clear();
            GUI.DrawWindow("Product Menu", 0, 0, new List<string>{
                "1. Show products",
                "2. Add product",
                "3. Edit product",
                "4. Delete product"
            });

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    _productService.ShowProducts();
                    break;
                case ConsoleKey.D2:
                    _productService.AddProduct();
                    break;
                case ConsoleKey.D3:
                    _productService.EditProduct();
                    break;
                case ConsoleKey.D4:
                    _productService.DeleteProduct();
                    break;
                default:
                    OutputHelper.ShowError("Invalid choice! Please try again...");
                    break;
            }
        }

        public void ShowCategoryAdminMenu()
        {
            Console.Clear();
            GUI.DrawWindow("Category Administration", 0, 0, new List<string>{
                "1. Show categories",
                "2. Add category",
                "3. Edit category",
                "4. Delete category"
            });


            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    _categoryService.ShowAllCategories();
                    break;
                case ConsoleKey.D2:
                    _categoryService.AddCategory();
                    break;
                case ConsoleKey.D3:
                    _categoryService.EditCategory();
                    break;
                case ConsoleKey.D4:
                    _categoryService.DeleteCategory();
                    break;
                default:
                    OutputHelper.ShowError("Invalid choice! Please try again...");
                    break;
            }
        }

        public void ShowCustomerAdminMenu()
        {
            Console.Clear();
            GUI.DrawWindow("Customer Administration", 0, 0, new List<string>
{
                "1. Show customers",
                "2. Add customer",
                "3. Edit customer",
                "4. Delete customer"
            });


            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    _customerService.ShowAllCustomers();
                    break;
                case ConsoleKey.D2:
                    _customerService.AddCustomer();
                    break;
                case ConsoleKey.D3:
                    _customerService.EditCustomer();
                    break;
                case ConsoleKey.D4:
                    _customerService.DeleteCustomer();
                    break;
                default:
                    OutputHelper.ShowError("Invalid choice! Please try again...");
                    break;
            }
        }

        public void ShowSupplierAdminMenu()
        {
            Console.Clear();
            GUI.DrawWindow("Supplier Administration", 0, 0, new List<string>
            {
                "1. Show all suppliers",
                "2. Add Supplier",
                "3. Edit Supplier",
                "4. Delete Supplier"
            });

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    _supplierService.ShowAllSuppliers();
                    break;
                case ConsoleKey.D2:
                    _supplierService.AddSupplier();
                    break;
                case ConsoleKey.D3:
                    _supplierService.EditSupplier();
                    break;
                case ConsoleKey.D4:
                    _supplierService.DeleteSupplier();
                    break;
                default:
                    OutputHelper.ShowError("Invalid choice! Please try again...");
                    break;
            }
        }
    }
}