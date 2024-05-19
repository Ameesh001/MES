// REFERENCIAS
using Microsoft.EntityFrameworkCore;
using PointOfSale.Data.DBContext;
using PointOfSale.Model;

namespace PointOfSale.Data.Repository
{
	public class GatePassReport : GenericRepository<Sale>, IGatePassReport
	{
		private readonly POINTOFSALEContext _dbcontext;
		public GatePassReport(POINTOFSALEContext context) : base(context)
		{
			_dbcontext = context;
		}

		public async Task<Sale> Register(Sale entity)
		{

			Sale SaleGenerated = new Sale();
			using (var transaction = _dbcontext.Database.BeginTransaction())
			{
				try
				{
					foreach (DetailSale dv in entity.DetailSales)
					{
						Product product_found = _dbcontext.Products.Where(p => p.IdProduct == dv.IdProduct).First();

						product_found.Quantity = product_found.Quantity - dv.Quantity;
						_dbcontext.Products.Update(product_found);
					}
					await _dbcontext.SaveChangesAsync();

					CorrelativeNumber correlative = _dbcontext.CorrelativeNumbers.Where(n => n.Management == "Sale").First();

					correlative.LastNumber = correlative.LastNumber + 1;
					correlative.DateUpdate = DateTime.Now;

					_dbcontext.CorrelativeNumbers.Update(correlative);
					await _dbcontext.SaveChangesAsync();


					string ceros = string.Concat(Enumerable.Repeat("0", correlative.QuantityDigits.Value));
					string saleNumber = ceros + correlative.LastNumber.ToString();
					saleNumber = saleNumber.Substring(saleNumber.Length - correlative.QuantityDigits.Value, correlative.QuantityDigits.Value);

					entity.SaleNumber = saleNumber;

					await _dbcontext.Sales.AddAsync(entity);
					await _dbcontext.SaveChangesAsync();

					SaleGenerated = entity;

					transaction.Commit();
				}
				catch (Exception ex)
				{
					transaction.Rollback();
					throw;
				}
			}

			return SaleGenerated;
		}

		public async Task<List<GatePass>> Report(DateTime StarDate, DateTime EndDate)
		{
			//List<DetailSale> listSummary = await _dbcontext.DetailSales
			//    .Include(v => v.IdSaleNavigation)
			//    .ThenInclude(u => u.IdUsersNavigation)
			//    .Include(v => v.IdSaleNavigation)
			//    .ThenInclude(tdv => tdv.IdTypeDocumentSaleNavigation)
			//    .Where(dv => dv.IdSaleNavigation.RegistrationDate.Value.Date >= StarDate.Date && dv.IdSaleNavigation.RegistrationDate.Value.Date <= EndDate.Date)
			//    .ToListAsync();

			//List<GatePass> listSummary2 = await _dbcontext.GatePasses.ToListAsync();
		

			var q = (from g in _dbcontext.GatePasses
					 join u in _dbcontext.Users on g.userID equals u.IdUsers into empDept
					 from ed in empDept.DefaultIfEmpty()
					 select new GatePass
					 {
						 dateGP = g.dateGP,
						 idGatePass = g.idGatePass,
						 name = g.name,
						 companyName = g.companyName,
						 contactNo = g.contactNo,
						 nic = g.nic,
						 vechicleType = g.vechicleType,
						 vechicleNo = g.vechicleNo,
						 remarks = g.remarks,
						 userID = g.userID,
						 Status = g.Status,
						 itemDetail = g.itemDetail,
						 item = ed.Name,
                         checkIn = g.checkIn,
                         checkOut = g.checkOut


					 }).Where(dv => dv.dateGP.Value.Date >= StarDate.Date && dv.dateGP.Value.Date <= EndDate.Date).ToListAsync();


			return await q;
		}
	}
}

