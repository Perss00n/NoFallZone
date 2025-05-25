using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces
{
    public interface IProductService
    {
        Task ShowShopProductsAsync();
        Task SearchProductsAsync();
        void ShowProductDetails(Product product);
        Task ShowDealsAsync();
        Task AddProductAsync();
        Task EditProductAsync();
        Task DeleteProductAsync();
    }
}
