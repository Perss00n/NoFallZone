using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Services;
public interface ISupplierService
{
    void ShowAllSuppliers();
    void AddSupplier();
    void EditSupplier();
    void DeleteSupplier();
}
