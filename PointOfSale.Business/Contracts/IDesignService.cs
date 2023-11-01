using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface IDesignService
    {
        Task<List<Design>> List();
        Task<Design> Add(Design entity);
        Task<Design> Edit(Design entity);
        Task<bool> Delete(int idCategory);
    }
}
