using Microsoft.EntityFrameworkCore;
using PointOfSale.Business.Contracts;
using PointOfSale.Data.Repository;
using PointOfSale.Model;
using System.Globalization;

namespace PointOfSale.Business.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly ISaleRepository _repositorySale;
        private readonly IGenericRepository<GatePass> _repositoryGatePass;
        private readonly IGenericRepository<DetailSale> _repositoryDetailSale;
        private readonly IGenericRepository<Category> _repositoryCategory;
        private readonly IGenericRepository<Product> _repositoryProduct;
        private DateTime StartDate = DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy"));

        public DashBoardService(
            ISaleRepository repositorySale,
            IGenericRepository<GatePass> repositoryGatePass,
            IGenericRepository<DetailSale> repositoryDetailSale,
            IGenericRepository<Category> repositoryCategory,
            IGenericRepository<Product> repositoryProduct
            )
        {

            _repositorySale = repositorySale;
            _repositoryGatePass = repositoryGatePass;
            _repositoryDetailSale = repositoryDetailSale;
            _repositoryCategory = repositoryCategory;
            _repositoryProduct = repositoryProduct;

            //  StartDate = StartDate.AddDays(-7);

        }
        public async Task<int> TotalSalesLastWeek()
        {
            try
            {
                IQueryable<Sale> query = await _repositorySale.Query(v => v.RegistrationDate.Value.Date >= StartDate.Date);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }


        public async Task<string> TotalIncomeLastWeek()
        {
            try
            {
                IQueryable<Sale> query = await _repositorySale.Query(v => v.RegistrationDate.Value.Date >= StartDate.Date);

                decimal resultado = query
                    .Select(v => v.Total)
                    .Sum(v => v.Value);

                return Convert.ToString(resultado, new CultureInfo("es-PE"));
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TotalProducts()
        {
            try
            {
                IQueryable<Product> query = await _repositoryProduct.Query();
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> TotalCategories()
        {
            try
            {
                IQueryable<Category> query = await _repositoryCategory.Query();
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }
        public async Task<Dictionary<string, int>> SalesLastWeek()
        {
            try
            {
                IQueryable<Sale> query = await _repositorySale.Query(v => v.RegistrationDate.Value.Date >= StartDate.Date);

                Dictionary<string, int> resultado = query
                    .GroupBy(v => v.RegistrationDate.Value.Date).OrderByDescending(g => g.Key)
                    .Select(dv => new { date = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.date, elementSelector: r => r.total);

                return resultado;

            }
            catch
            {
                throw;
            }
        }
        public async Task<Dictionary<string, int>> ProductsTopLastWeek()
        {
            try
            {
                IQueryable<DetailSale> query = await _repositoryDetailSale.Query();

                Dictionary<string, int> resultado = query
                    .Include(v => v.IdSaleNavigation)
                    .Where(dv => dv.IdSaleNavigation.RegistrationDate.Value.Date >= StartDate)
                    .GroupBy(dv => dv.DescriptionProduct).OrderByDescending(g => g.Count())
                    .Select(dv => new { product = dv.Key, total = dv.Count() }).Take(4)
                    .ToDictionary(keySelector: r => r.product, elementSelector: r => r.total);

                return resultado;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> SameDayPasses()
        {
            try
            {
                IQueryable<GatePass> query = await _repositoryGatePass.Query(v => v.dateGP.Value.Date >= StartDate.Date);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> SuccessReturned()
        {
            try
            {
                IQueryable<GatePass> query = await _repositoryGatePass.Query(v => v.checkIn.Value.Date >= StartDate.Date && v.isReceived == 1);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> TodayPending()
        {
            try
            {
                IQueryable<GatePass> query = await _repositoryGatePass.Query(v => v.checkIn.Value.Date == StartDate.Date && v.isReceived == 0);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }
        public async Task<int> TotalPending()
        {
            try
            {
                IQueryable<GatePass> query = await _repositoryGatePass.Query(v => v.isReceived == 0);
                int total = query.Count();
                return total;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> PassesLastWeek()
        {
            try
            {
                IQueryable<GatePass> query = await _repositoryGatePass.Query(v => v.dateGP.Value.Date >= StartDate.Date);

                Dictionary<string, int> resultado = query
                    .GroupBy(v => v.dateGP.Value.Date).OrderByDescending(g => g.Key)
                    .Select(dv => new { date = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.date, elementSelector: r => r.total);

                return resultado;

            }
            catch
            {
                throw;
            }
        }

        public async Task<Dictionary<string, int>> TopStatuses()
        {
            try
            {
                IQueryable<GatePass> query = await _repositoryGatePass.Query();

                Dictionary<string, int> resultado = query
                    .Where(dv => dv.dateGP.Value.Date >= StartDate)
                    .GroupBy(dv => dv.Status).OrderByDescending(g => g.Count())
                    .Select(dv => new { product = dv.Key, total = dv.Count() }).Take(4)
                    .ToDictionary(keySelector: r => r.product, elementSelector: r => r.total);

                return resultado;
            }
            catch
            {
                throw;
            }
        }
    }
}
