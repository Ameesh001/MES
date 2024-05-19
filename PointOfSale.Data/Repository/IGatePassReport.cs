using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Data.Repository
{
    public interface IGatePassReport : IGenericRepository<Sale>
    {
        Task<Sale> Register(Sale entity);

        Task<List<GatePass>> Report(DateTime StartDate, DateTime EndDate);

    }
}
