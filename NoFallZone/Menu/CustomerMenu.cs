using NoFallZone.Services.Implementations;
using NoFallZone.Services.Interfaces;

namespace NoFallZone.Menu
{
    public class CustomerMenu
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CustomerMenu(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public List<string> GetMenuItems()
        {
            return new List<string>
            {
                "[E] Enter Shop",
                "[C] Cart",
                "[S] Search",
                "[Q] Logout"
            };
        }

        public void ShowShop() => _productService.ShowShopProducts();
        public void Search() => _productService.SearchProducts();
        public void OpenCart() => _cartService.OpenCartMenu();

    }
}