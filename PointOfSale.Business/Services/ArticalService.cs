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
    public class ArticalService : IArticalService
    {
        private readonly IGenericRepository<Artical> _repository;
        public ArticalService(IGenericRepository<Artical> repository)
        {
            _repository = repository;
        }

        public async Task<List<Artical>> List()
        {
            IQueryable<Artical> query = await _repository.Query();
            return query.ToList();
        }

        public async Task<Artical> Add(Artical entity)
        {
            try
            {
                Artical category_created = await _repository.Add(entity);
                if (category_created.IdCategory == 0)
                    throw new TaskCanceledException("Artical could not be created");

                return category_created;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Artical> Edit(Artical entity)
        {
            try
            {
                Artical category_found = await _repository.Get(c => c.IdCategory == entity.IdCategory);

                category_found.Description = entity.Description;
                category_found.IsActive = entity.IsActive;

                bool response = await _repository.Edit(category_found);

                if (!response)
                    throw new TaskCanceledException("Artical could not be changed.");

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
                Artical category_found = await _repository.Get(c => c.IdCategory == idCategory);

                if (category_found == null)
                    throw new TaskCanceledException("The Artical does not exist");


                bool response = await _repository.Delete(category_found);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

      

    }
}
