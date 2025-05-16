using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFallZone.Models;

namespace NoFallZone.Services
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
