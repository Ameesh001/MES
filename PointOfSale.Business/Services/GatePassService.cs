using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using PointOfSale.Business.Contracts;
using PointOfSale.Data.Repository;
using PointOfSale.Model;

namespace PointOfSale.Business.Services
{
    public class GatePassService : IGatePassService
    {
		private readonly IGatePassReport _repositorySale;
		private readonly IGenericRepository<GatePass> _repository;
        public GatePassService(IGenericRepository<GatePass> repository, IGatePassReport repositorySale)
        {
            _repository = repository;
			_repositorySale = repositorySale;
		}

        public async Task<List<GatePass>> List()
        {
            IQueryable<GatePass> query = await _repository.Query();
            return query.ToList();
        }

        public async Task<GatePass> Add(GatePass entity)
        {
            try
            {
                GatePass category_created = await _repository.Add(entity);
                if (category_created.idGatePass == 0)
                    throw new TaskCanceledException("GatePass could not be created");

                return category_created;
            }
            catch
            {
                throw;
            }
        }

        public async Task<GatePass> Edit(GatePass entity)
        {
            try
            {
                GatePass category_found = await _repository.Get(c => c.idGatePass == entity.idGatePass);

                category_found.name = entity.name;
                category_found.companyName = entity.companyName;
				category_found.contactNo = entity.contactNo;
				category_found.nic = entity.nic;
				category_found.vechicleType = entity.vechicleType;
				category_found.vechicleNo = entity.vechicleNo;
				category_found.Status = entity.Status;
				category_found.remarks = entity.remarks;
				category_found.userID = entity.userID;
				category_found.item = entity.item;
				category_found.itemDetail = entity.itemDetail;
                category_found.checkIn = entity.checkIn;
                category_found.checkOut = entity.checkOut;
                category_found.isReceived = entity.isReceived;
                // category_found.dateGP = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));



                bool response = await _repository.Edit(category_found);

                if (!response)
                    throw new TaskCanceledException("GatePass could not be changed.");

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
                GatePass category_found = await _repository.Get(c => c.idGatePass == idCategory);

                if (category_found == null)
                    throw new TaskCanceledException("The GatePass does not exist");


                bool response = await _repository.Delete(category_found);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

		public async Task<List<GatePass>> Report(string StartDate, string EndDate)
		{
			DateTime start_date = DateTime.ParseExact(StartDate, "dd/MM/yyyy", new CultureInfo("es-PE"));
			DateTime end_date = DateTime.ParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("es-PE"));

			List<GatePass> lista = await _repositorySale.Report(start_date, end_date);

			return lista;
		}

        public async Task<GatePass> GetSingle(int recordID)
        {
            IQueryable<GatePass> query = await _repository.Query(v => v.idGatePass == recordID);
            return query.First();
        }
    }
}
