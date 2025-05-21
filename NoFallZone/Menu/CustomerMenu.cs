using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

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
                "[E] Enter Shop",
                "[C] Cart",
                "[S] Search"
            };
        }

        public void ShowShop()
        {
            _productService.ShowShopProducts();
        }

        public void OpenCart()
        {

            if (Session.Cart.Count == 0)
            {
                Console.Clear();
                OutputHelper.ShowInfo("Your cart is empty!");
                return;
            }

            bool inCartMenu = true;

            while (inCartMenu)
            {
                CartHelper.PrintCartItems();

                var choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.C:
                        CartHelper.ChangeCartQuantity();
                        break;

                    case ConsoleKey.R:
                        if (CartHelper.DeleteItemFromCart(out string message))
                        {
                            OutputHelper.ShowSuccess(message);
                            inCartMenu = Session.Cart.Count <= 0 ? false : true;
                        }
                        else
                        {
                            OutputHelper.ShowError(message);
                        }
                        break;

                    case ConsoleKey.Q:
                        inCartMenu = false;
                        break;

                    default:
                        OutputHelper.ShowError("Invalid option. Try again!");
                        break;
                }

                if (inCartMenu)
                {
                    OutputHelper.ShowInfo("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        public void Search()
        {
            _productService.SearchProducts();
        }

    }
}