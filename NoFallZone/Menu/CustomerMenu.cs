using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NoFallZone.Services;

namespace NoFallZone.Menu
{
    public class CustomerMenu
    {
        private readonly IProductService _productService;

        public CustomerMenu(IProductService productService)
        {
            _productService = productService;
        }

        public List<string> GetMenuItems()
        {
            return new List<string>
            {
                "[H] Home Page",
                "[E] Enter Shop",
                "[C] Cart",
                "[S] Search"
            };
        }

        public void ShowShop()
        {
            Console.Clear();
            _productService.ShowProducts();
        }

        public void OpenCart()
        {
            Console.Clear();
            Console.WriteLine("Cart not implemented yet...");
        }

        public void GoHome()
        {
            Console.Clear();
            Console.WriteLine("Home Page not implemented yet...");
        }

        public void Search()
        {
            _productService.SearchProducts();
        }
    }
}