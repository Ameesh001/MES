using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface IStyleService
    {
        Task<List<Style>> List();
        Task<Style> Add(Style entity);
        Task<Style> Edit(Style entity);
        Task<bool> Delete(int idCategory);
    }
}
