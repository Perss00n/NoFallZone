using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Services.Implementations;
using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new NoFallZoneContext();
            //SeedData.ClearDatabase(db);
            //SeedData.Initialize(db);
            IProductService productService = new ProductService(db);
            ICustomerService customerService = new CustomerService(db);
            ICategoryService categoryService = new CategoryService(db);
            ISupplierService supplierService = new SupplierService(db);

            var adminMenu = new AdminMenu(productService, customerService, categoryService, supplierService);
            var customerMenu = new CustomerMenu(productService);

            bool running = true;

            while (running)
            {
                DisplayHelper.ShowStartPage();
                Console.CursorVisible = false;
                var choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.D1:
                        var user = LoginHelper.LoginUser(db);
                        if (user == null) break;

                        Session.LoggedInUser = user;

                        var startPage = new StartPage(
                            productService,
                            customerService,
                            categoryService,
                            supplierService,
                            customerMenu,
                            Session.IsAdmin ? adminMenu : null
                        );

                        startPage.Show();
                        break;

                    case ConsoleKey.D2:
                        RegistrationHelper.RegisterNewCustomer(db);
                        break;

                    case ConsoleKey.D3:
                        running = false;
                        break;

                    default:
                        Console.Clear();
                        OutputHelper.ShowError("Invalid choice! Please try again...");
                        Thread.Sleep(1500);
                        break;
                }
            }

            Console.Clear();
            OutputHelper.ShowInfo("Thank you for visiting NoFallZone! L8terZ!");

        }
    }
}