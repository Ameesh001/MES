using Microsoft.EntityFrameworkCore;
using PointOfSale.Business.Contracts;
using PointOfSale.Data.DBContext;
using PointOfSale.Data.Repository;
using PointOfSale.Model;

namespace PointOfSale.Business.Services
{
    public class ChequeService : IChequeService
    {
        private readonly IGenericRepository<Cheque> _repository;

        private readonly POINTOFSALEContext _dbcontext;
        public ChequeService(IGenericRepository<Cheque> repository, POINTOFSALEContext dbcontext)
        {
            _repository = repository;
            _dbcontext = dbcontext;

        }

        //public async Task<List<Cheque>> List()
        //{
        //    IQueryable<Cheque> query = await _repository.Query();
        //    return query.ToList();
        //}
        public async Task<List<Cheque>> List()
        {
            //List<DetailSale> listSummary = await _dbcontext.DetailSales
            //    .Include(v => v.IdSaleNavigation)
            //    .ThenInclude(u => u.IdUsersNavigation)
            //    .Include(v => v.IdSaleNavigation)
            //    .ThenInclude(tdv => tdv.IdTypeDocumentSaleNavigation)
            //    .Where(dv => dv.IdSaleNavigation.RegistrationDate.Value.Date >= StarDate.Date && dv.IdSaleNavigation.RegistrationDate.Value.Date <= EndDate.Date)
            //    .ToListAsync();

            //List<GatePass> listSummary2 = await _dbcontext.GatePasses.ToListAsync();


            var q = (from g in _dbcontext.Cheques
                     join u in _dbcontext.Banks on g.bank equals u.IdCategory into empDept
                     from ed in empDept.DefaultIfEmpty()
                     select new Cheque
                     {
                         idcheque = g.idcheque,
                         amountInWords = g.amountInWords,
                         sysdate = g.sysdate,
                         payeeName = g.payeeName,
                         amount = g.amount,
                         chequeNo = g.chequeNo,
                         depositDate = g.depositDate,
                         bank = g.bank,
                         States = g.States,
                         userID = g.userID,
                         CheqNoLeft = ed.Description,
                         chequeType = g.chequeType

                     }).OrderByDescending(x => x.idcheque).ToListAsync();


            return await q;
        }

        public async Task<Cheque> Add(Cheque entity)
        {
            try
            {
                //entity.editDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                Cheque category_created = await _repository.Add(entity);
                if (category_created.idcheque == 0)
                    throw new TaskCanceledException("Cheque could not be created");

                return category_created;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Cheque> Edit(Cheque entity)
        {
            try
            {
                Cheque category_found = await _repository.Get(c => c.idcheque == entity.idcheque);

                category_found.payeeName = entity.payeeName;
                category_found.amount = entity.amount;
                category_found.chequeNo = entity.chequeNo;
                category_found.amountInWords = entity.amountInWords;
                category_found.depositDate = entity.depositDate;
                category_found.bank = entity.bank;
                category_found.States = entity.States;
                category_found.nameTop = entity.nameTop;
                category_found.nameLeft = entity.nameLeft;
                category_found.accTop = entity.accTop;
                category_found.accLeft = entity.accLeft;
                category_found.DateLeft = entity.DateLeft;
                category_found.DateTop = entity.DateTop;
                category_found.CheqNoTop = entity.CheqNoTop;
                category_found.CheqNoLeft = entity.CheqNoLeft;
                category_found.wordsLeft = entity.wordsLeft;
                category_found.wordsTop = entity.wordsTop;
                category_found.userID = entity.userID;
                category_found.chequeType = entity.chequeType;
                category_found.editDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));



                bool response = await _repository.Edit(category_found);

                if (!response)
                    throw new TaskCanceledException("Cheque could not be changed.");

                return category_found;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Delete(int idCategory)
        {
            try
            {
                Cheque category_found = await _repository.Get(c => c.idcheque == idCategory);

                if (category_found == null)
                    throw new TaskCanceledException("The Cheque does not exist");


                bool response = await _repository.Delete(category_found);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public async Task<List<Cheque>> Report(string StartDate, string EndDate)
        //{
        //	DateTime start_date = DateTime.ParseExact(StartDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
        //	DateTime end_date = DateTime.ParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("es-PE"));

        //	List<Cheque> lista = await _repositorySale.Report(start_date, end_date);

        //	return lista;
        //}
        public Cheque GetSingleData(string recordID)
        {
            //IQueryable<Cheque> query = await _repository.Query(v => v.bank == int.Parse(recordID));
            Cheque bank_found = _dbcontext.Cheques.Where(p => p.bank == int.Parse(recordID)).OrderByDescending(x => x.editDate).First();
            return bank_found;
        }

        public dynamic GetSingle(string recordID)
        {
            //IQueryable<Cheque> query = await _repository.Query(v => v.bank == recordID);
            //Cheque bank_found = _dbcontext.Cheques.Where(p => p.bank == int.Parse(recordID)).OrderByDescending(x => x.editDate).First();

            var q = (from c in _dbcontext.Cheques
                     join b in _dbcontext.Banks on c.bank equals b.IdCategory into empDept
                     from ed in empDept.DefaultIfEmpty()
                     select new
                     {
                         c.wordsTop,
                         c.wordsLeft,
                         c.DateLeft,
                         c.DateTop,
                         c.accLeft,
                         c.accTop,
                         c.nameLeft,
                         c.nameTop,
                         ed.wordsTopB,
                         ed.wordsLeftB,
                         ed.DateLeftB,
                         ed.DateTopB,
                         ed.accLeftB,
                         ed.accTopB,
                         ed.nameLeftB,
                         ed.nameTopB,
                         c.bank,
                         c.editDate,

                     }).Where(p => p.bank == int.Parse(recordID)).OrderByDescending(x => x.editDate).First();
            return q;
        }

        public async Task<Cheque> GetSinglePDF(int recordID)
        {
            IQueryable<Cheque> query = await _repository.Query(v => v.idcheque == recordID);
            return query.First();
        }

        //Task<Cheque> GetSinglePDF(int recordID);
        //{
        //    try
        //    {
        //        log.LogWriter("PDFGatePass");
        //        VMGatePass vmVenta = _mapper.Map<VMGatePass>(await _gatePassService.GetSingle(recordID));
        //        return View(vmVenta);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.LogWriter("PDFGatePass Exception: " + ex);
        //        throw;
        //    }

        //}
    }
}
