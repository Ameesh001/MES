using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface ISizeService
    {
        Task<List<Size>> List();
        Task<Size> Add(Size entity);
        Task<Size> Edit(Size entity);
        Task<bool> Delete(int idCategory);
    }
}
