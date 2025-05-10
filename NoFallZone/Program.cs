using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Services;
using NoFallZone.Utilities;

namespace NoFallZone
{
    public class Program
    {
        static void Main(string[] args)
        {
            var db = new NoFallZoneContext();
            IProductService productService = new ProductService(db);
            ICustomerService customerService = new CustomerService(db);


            var menu = new MainMenu(productService, customerService);
            menu.Show();



            //SeedData.ClearDatabase(db);
            //SeedData.Initialize(db);
        }
    }
}