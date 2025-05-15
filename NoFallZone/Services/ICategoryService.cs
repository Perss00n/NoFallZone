using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Services;
public interface ICategoryService
{
    void ShowAllCategories();
    void AddCategory();
    void EditCategory();
    void DeleteCategory();
}
