using NoFallZone.Services;
using NoFallZone.Utilities;

namespace NoFallZone.Menu
{
    public class AdminMenu
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ICategoryService _categoryService;
        //private readonly IStatsService _statsService;

        public AdminMenu(IProductService productService, ICustomerService customerService, ICategoryService categoryService/*, IStatsService statsService*/)
        {
            _productService = productService;
            _customerService = customerService;
            _categoryService = categoryService;
            //_statsService = statsService;
        }

        public List<string> GetMenuItems()
        {
            return new List<string>
        {
                    "1. Administrera produkter",
                    "2. Administrera kategorier",
                    "3. Administrera kunder",
                    "4. Se statistik(Queries)",
                    "5. Tillbaka till inloggning"
        };
        }

        public void ShowProductAdminMenu()
        {
            Console.Clear();
            GUI.DrawWindow("Produktmeny", 0, 0, new List<string>
            {
                "1. Visa produkter",
                "2. Lägg till produkt",
                "3. Redigera produkt",
                "4. Ta bort produkt"
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
                    OutputHelper.ShowError("Ogiltigt val i produktmenyn! Försök igen...");
                    break;
            }
        }

        public void ShowCategoryAdminMenu()
        {
            Console.Clear();
            GUI.DrawWindow("Kategoriadministration", 0, 0, new List<string>
            {
                "1. Visa kategorier",
                "2. Lägg till kategori",
                "3. Redigera kategori",
                "4. Ta bort kategori"
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
                    OutputHelper.ShowError("Ogiltigt val i kategorimenyn! Försök igen...");
                    break;
            }
        }

        public void ShowCustomerAdminMenu()
        {
            Console.Clear();
            GUI.DrawWindow("Kundadministration", 0, 0, new List<string>
            {
                "1. Visa kunder",
                "2. Lägg till kund",
                "3. Redigera kund",
                "4. Ta bort kund"
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
                    OutputHelper.ShowError("Ogiltigt val i kundmenyn! Försök igen...");
                    break;
            }
        }

        public void ShowStatisticsMenu()
        {
            Console.Clear();
            GUI.DrawWindow("Statistik & Queries", 0, 0, new List<string>
            {
                "1. Visa antal produkter per kategori",
                "2. Visa kunder med flest ordrar",
                "3. Visa mest sålda produkter",
                "4. Visa total försäljning"
            });

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.D1:
                    //_statsService.ShowProductCountPerCategory();
                    break;
                case ConsoleKey.D2:
                    //_statsService.ShowTopCustomers();
                    break;
                case ConsoleKey.D3:
                    //_statsService.ShowTopSellingProducts();
                    break;
                case ConsoleKey.D4:
                    //_statsService.ShowTotalSales();
                    break;
                default:
                    OutputHelper.ShowError("Ogiltigt val i statistikmenyn! Försök igen...");
                    break;
            }
        }
    }
}