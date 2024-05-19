using PointOfSale.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Business.Contracts
{
    public interface IChequeService
    {
        Task<List<Cheque>> List();
        Task<Cheque> Add(Cheque entity);
        Task<Cheque> Edit(Cheque entity);
        Task<bool> Delete(int idCategory);
        dynamic GetSingle(string recordID);
        Cheque GetSingleData(string recordID);
        Task<Cheque> GetSinglePDF(int recordID);
    }
}
