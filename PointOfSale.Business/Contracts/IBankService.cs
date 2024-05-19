using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface IBankService
    {
        Task<List<Bank>> List();
        Task<List<Bank>> ListActive();
        Task<Bank> Add(Bank entity);
        Task<Bank> Edit(Bank entity);
        Task<bool> Delete(int idCategory);
    }
}
