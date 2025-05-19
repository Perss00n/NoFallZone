using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces
{
    public interface IProductService
    {
        void ShowProducts();
        void ShowShopProducts();
        void SearchProducts();
        void ShowProductDetails(Product product);
        void ShowDeals();
        void AddDealToCart(ConsoleKey dealKey);
        void AddProduct();
        void EditProduct();
        void DeleteProduct();
    }
}
