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
    public class StyleService : IStyleService
    {
        private readonly IGenericRepository<Style> _repository;
        public StyleService(IGenericRepository<Style> repository)
        {
            _repository = repository;
        }

        public async Task<List<Style>> List()
        {
            IQueryable<Style> query = await _repository.Query();
            return query.ToList();
        }

        public async Task<Style> Add(Style entity)
        {
            try
            {
                Style category_created = await _repository.Add(entity);
                if (category_created.IdCategory == 0)
                    throw new TaskCanceledException("Style could not be created");

                return category_created;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Style> Edit(Style entity)
        {
            try
            {
                Style category_found = await _repository.Get(c => c.IdCategory == entity.IdCategory);

                category_found.Description = entity.Description;
                category_found.IsActive = entity.IsActive;

                bool response = await _repository.Edit(category_found);

                if (!response)
                    throw new TaskCanceledException("Style could not be changed.");

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
                Style category_found = await _repository.Get(c => c.IdCategory == idCategory);

                if (category_found == null)
                    throw new TaskCanceledException("The Style does not exist");


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
