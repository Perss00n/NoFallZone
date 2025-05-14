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
        private readonly ICustomerService _customerService;

        public CustomerMenu(IProductService productService, ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
        }

        public List<string> GetMenuItems()
        {
            return new List<string>
            {
                "[H] Home Page",
                "[S] Shop",
                "[C] Cart"
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
    }
}