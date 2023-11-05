using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface ICustomerService
    {
        Task<List<Customer>> List();
        Task<Customer> Add(Customer entity);
        Task<Customer> Edit(Customer entity);
        Task<bool> Delete(int idCustomer);
    }
}
