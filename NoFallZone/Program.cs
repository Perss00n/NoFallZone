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
            using var db = new NoFallZoneContext();

            //SeedData.Initialize(db);


            var productService = new ProductService(db);

            var menu = new MainMenu(productService);
            menu.Show();
        }
    }
}