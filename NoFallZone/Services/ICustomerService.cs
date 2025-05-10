using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Services;
public interface ICustomerService
{
    void ShowAllCustomers();
    void AddCustomer();
    void EditCustomer();
    void DeleteCustomer();
}
