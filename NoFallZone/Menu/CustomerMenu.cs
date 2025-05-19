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

                GUI.DrawWindow("Your Cart", 1, 1, lines, maxLineWidth: 50);

                GUI.DrawWindow("Options", 1, lines.Count + 3, new List<string>
                {
                    "[C] Change quantity",
                    "[R] Remove product",
                    "[Q] Return to menu"
                });

                var choice = Console.ReadKey(true).Key;

                switch (choice)
                {
                    case ConsoleKey.C:
                        int indexToChange = InputHelper.PromptInt("\nEnter item number to change quantity", 1, Session.Cart.Count,
                            $"Choose a number between 1 and {Session.Cart.Count}") - 1;

                        var itemToChange = Session.Cart[indexToChange];
                        int newQty = InputHelper.PromptInt($"\nEnter new quantity for {itemToChange.Product.Name}", 1, itemToChange.Product.Stock,
                            $"Enter a number between 1 and {itemToChange.Product.Stock}");

                        itemToChange.Quantity = newQty;
                        OutputHelper.ShowSuccess($"Updated quantity of {itemToChange.Product.Name} to {newQty}");
                        break;

                    case ConsoleKey.R:                        
                            int itemIndex = InputHelper.PromptInt("\nEnter item number to remove", 1, Session.Cart.Count,
                                $"Choose a number between 1 and {Session.Cart.Count}") - 1;

                            var item = Session.Cart[itemIndex];
                            bool confirmed = InputHelper.PromptYesNo(
                                $"Are you sure you want to remove {item.Product.Name} from your cart?",
                                "Please answer 'Y' for Yes or 'N' for No!");

                            if (!confirmed)
                            {
                                OutputHelper.ShowInfo("The action was cancelled!");
                                break;
                            }

                            Session.Cart.RemoveAt(itemIndex);
                            OutputHelper.ShowSuccess($"{item.Product.Name} removed from cart!");
                            inCartMenu = false;
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