using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface IColourService
    {
        Task<List<Colour>> List();
        Task<Colour> Add(Colour entity);
        Task<Colour> Edit(Colour entity);
        Task<bool> Delete(int idCategory);
    }
}
