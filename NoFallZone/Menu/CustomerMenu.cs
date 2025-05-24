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
            return
            [
                "[E] Enter Shop",
                "[C] Cart",
                "[S] Search",
                "[Q] Logout"
            ];
        }

        public async Task ShowShopAsync() => await _productService.ShowShopProductsAsync();
        public async Task SearchAsync() => await _productService.SearchProductsAsync();
        public async Task OpenCartAsync() => await _cartService.OpenCartMenuAsync();

    }
}