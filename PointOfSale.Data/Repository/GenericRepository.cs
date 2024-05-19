using Microsoft.EntityFrameworkCore;
using PointOfSale.Data.DBContext;
using PointOfSale.Data.LogMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly POINTOFSALEContext _dbcontext;
        Loggers _log = new Loggers();
        public GenericRepository(POINTOFSALEContext context)
        {
            _dbcontext = context;
        }
        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                TEntity entity = await _dbcontext.Set<TEntity>().FirstOrDefaultAsync(filter);
                return entity;
            }
            catch(Exception ex)
            {
                _log.LogWriter("GenericRepo Get ex: " + ex);
                throw;
            }
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            try
            {

                _dbcontext.Set<TEntity>().Add(entity);
                await _dbcontext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _log.LogWriter("GenericRepo Add ex: " + ex);
                throw;
            }
        }

        public async Task<bool> Edit(TEntity entity)
        {
            try
            {
                _dbcontext.Update(entity);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogWriter("GenericRepo Edit ex: " + ex);
                throw;
            }
        }

        public async Task<bool> Delete(TEntity entity)
        {
            try
            {
                _dbcontext.Remove(entity);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _log.LogWriter("GenericRepo Delete ex: " + ex);
                throw;
            }
        }

        public Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> filter)
        {

            try
            {
                IQueryable<TEntity> queryentity = filter == null ? _dbcontext.Set<TEntity>() : _dbcontext.Set<TEntity>().Where(filter);
                return Task.FromResult(queryentity);
            }
            catch (Exception ex)
            {
                _log.LogWriter("GenericRepo Query ex: " + ex);
                throw;
            }
        }
    }
}
