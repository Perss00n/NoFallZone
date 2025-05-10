using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Services
{
    public interface IProductService
    {
        void ShowAllProducts();
        void ShowDeals();
        void AddProduct();
        void EditProduct();
        void DeleteProduct();
    }
}
