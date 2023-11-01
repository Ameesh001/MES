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
    public class ColourService : IColourService
    {
        private readonly IGenericRepository<Colour> _repository;
        public ColourService(IGenericRepository<Colour> repository)
        {
            _repository = repository;
        }

        public async Task<List<Colour>> List()
        {
            IQueryable<Colour> query = await _repository.Query();
            return query.ToList();
        }

        public async Task<Colour> Add(Colour entity)
        {
            try
            {
                Colour category_created = await _repository.Add(entity);
                if (category_created.IdCategory == 0)
                    throw new TaskCanceledException("Colour could not be created");

                return category_created;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Colour> Edit(Colour entity)
        {
            try
            {
                Colour category_found = await _repository.Get(c => c.IdCategory == entity.IdCategory);

                category_found.Description = entity.Description;
                category_found.IsActive = entity.IsActive;

                bool response = await _repository.Edit(category_found);

                if (!response)
                    throw new TaskCanceledException("Colour could not be changed.");

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
                Colour category_found = await _repository.Get(c => c.IdCategory == idCategory);

                if (category_found == null)
                    throw new TaskCanceledException("The Colour does not exist");


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
