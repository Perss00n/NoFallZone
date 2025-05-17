using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces
{
    public interface IProductService
    {
        void ShowProducts();
        void SearchProducts();
        void ShowProductDetails(Product product);
        void ShowDeals();
        void AddProduct();
        void EditProduct();
        void DeleteProduct();
    }
}
