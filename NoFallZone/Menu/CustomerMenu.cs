using NoFallZone.Services.Interfaces;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.Session;

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

            if (Session.Cart.Count == 0)
            {
                OutputHelper.ShowInfo("Your cart is empty!");
                return;
            }

            bool inCartMenu = true;

            while (inCartMenu)
            {
                Console.Clear();
                decimal total = 0;
                var lines = new List<string>();

                for (int i = 0; i < Session.Cart.Count; i++)
                {
                    var item = Session.Cart[i];
                    decimal itemTotal = item.Product.Price * item.Quantity;
                    total += itemTotal;

                    lines.Add($"{i + 1}. {item.Quantity} x {item.Product.Name} = {itemTotal:C}");
                }

                lines.Add("------------------------");
                lines.Add($"Total: {total:C}");
                GUI.DrawWindow("Your Cart", 5, 8, lines, maxLineWidth: 50);

                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("[C] Change quantity");
                Console.WriteLine("[R] Remove product");
                Console.WriteLine("[Q] Return to menu");

                var choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.C:
                        int indexToChange = InputHelper.PromptInt("Enter item number to change quantity", 1, Session.Cart.Count,
                            $"Choose a number between 1 and {Session.Cart.Count}") - 1;

                        var itemToChange = Session.Cart[indexToChange];
                        int newQty = InputHelper.PromptInt($"Enter new quantity for {itemToChange.Product.Name}", 1, itemToChange.Product.Stock,
                            $"Enter a number between 1 and {itemToChange.Product.Stock}");

                        itemToChange.Quantity = newQty;
                        OutputHelper.ShowSuccess($"Updated quantity of {itemToChange.Product.Name} to {newQty}");
                        break;

                    case ConsoleKey.R:
                        int indexToRemove = InputHelper.PromptInt("Enter item number to remove", 1, Session.Cart.Count,
                            $"Choose a number between 1 and {Session.Cart.Count}") - 1;

                        var itemToRemove = Session.Cart[indexToRemove];
                        Session.Cart.RemoveAt(indexToRemove);
                        OutputHelper.ShowSuccess($"{itemToRemove.Product.Name} removed from cart!");
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