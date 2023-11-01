using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface IArticalService
    {
        Task<List<Artical>> List();
        Task<Artical> Add(Artical entity);
        Task<Artical> Edit(Artical entity);
        Task<bool> Delete(int idCategory);
    }
}
