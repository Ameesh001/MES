using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PointOfSale.Business.Contracts;
using PointOfSale.Data.Repository;
using PointOfSale.Model;

namespace PointOfSale.Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGenericRepository<Customer> _repository;
        public CustomerService(IGenericRepository<Customer> repository)
        {
            _repository = repository;
        }

        public async Task<List<Customer>> List()
        {
            IQueryable<Customer> query = await _repository.Query();
			return query.Include(c => c.IdCategoryNavigation).ToList();
		}
        public async Task<Customer> Add(Customer entity)
        {
            //Customer Customer_exists = await _repository.Get(p => p.CusCode == entity.CusCode);

            //if (Customer_exists != null)
            //    throw new TaskCanceledException("The CusCode already exists");

            try
            {
                Customer Customer_created = await _repository.Add(entity);

                if (Customer_created.IdProduct == 0)
                    throw new TaskCanceledException("Failed to create Customer");

                IQueryable<Customer> query = await _repository.Query(p => p.IdProduct == Customer_created.IdProduct);
				Customer_created = query.Include(c => c.IdCategoryNavigation).First();
				

                return Customer_created;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Customer> Edit(Customer entity)
        {
            //Customer Customer_exists = await _repository.Get(p => p.CusCode == entity.CusCode && p.IdProduct != entity.IdProduct);

            //if (Customer_exists != null)
            //    throw new TaskCanceledException("The CusCode already exists");

            try
            {
                IQueryable<Customer> queryCustomer = await _repository.Query(u => u.IdProduct == entity.IdProduct);

                Customer Customer_edit = queryCustomer.First();

                Customer_edit.CusCode = entity.CusCode;
                Customer_edit.invoiceName = entity.invoiceName;
                Customer_edit.ShortName = entity.ShortName;
                Customer_edit.IdBank = entity.IdBank;
                Customer_edit.PhoneNo = entity.PhoneNo;
                Customer_edit.Mobile = entity.Mobile;
                Customer_edit.OpeningBalance = entity.OpeningBalance;
                Customer_edit.Address = entity.Address;
				Customer_edit.Debit = entity.Debit;
				if (entity.Photo != null && entity.Photo.Length > 0)
                    Customer_edit.Photo = entity.Photo;
                Customer_edit.IsActive = entity.IsActive;

                bool response = await _repository.Edit(Customer_edit);
                if (!response)
                    throw new TaskCanceledException("The Customer could not be modified");


				Customer Customer_edited = queryCustomer.Include(c => c.IdCategoryNavigation).First();

				return Customer_edited;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> Delete(int IdProduct)
        {
            try
            {
                Customer Customer_found = await _repository.Get(p => p.IdProduct == IdProduct);

                if (Customer_found == null)
                    throw new TaskCanceledException("The Customer does not exist");

                bool response = await _repository.Delete(Customer_found);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
