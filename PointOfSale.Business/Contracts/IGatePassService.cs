using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface IGatePassService
    {
        Task<List<GatePass>> List();
        Task<GatePass> Add(GatePass entity);
        Task<GatePass> Edit(GatePass entity);
        Task<bool> Delete(int idCategory);

		Task<List<GatePass>> Report(string StarDate, string EndDate);
        Task<GatePass> GetSingle(int recordID);
    }
}
