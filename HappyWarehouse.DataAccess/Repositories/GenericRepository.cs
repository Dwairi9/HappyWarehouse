using HappyWarehouse.BusinessLogic.Interfaces.IRepsitories;
using HappyWarehouse.DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HappyWarehouse.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Property
        private readonly HappyWarehouseDbContext _dbContext;
        #endregion

        public GenericRepository(HappyWarehouseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Method :: Add
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        #endregion

        #region Method :: Add Range
        /// <summary>
        /// AddRange
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            if (entities.Any())
            {
                await _dbContext.Set<T>().AddRangeAsync(entities);
                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion

        #region Method :: Remove
        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async Task RemoveAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Method :: Remove Range
        /// <summary>
        /// RemoveRange
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task RemoveRange(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Method :: Count
        /// <summary>
        /// Count
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            var query = _dbContext.Set<T>().Where(predicate);

            return await query.CountAsync();
        }
        #endregion

        #region Method :: Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Delete(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        #endregion

        #region Method :: Delete
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            if (entities.Any())
            {
                _dbContext.Set<T>().RemoveRange(entities);
                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion

        #region Method :: FirstOrDefault
        /// <summary>
        /// FirstOrDefault
        /// </summary>
        /// <returns></returns>
        public async Task<T> FirstOrDefaultAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            return await query.FirstOrDefaultAsync();
        }
        #endregion

        #region Method :: Get 
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region Method :: GetAll 
        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            if (orderBy != null)
                query = orderBy(query);

            return await query.ToListAsync();
        }
        #endregion

        #region Method :: GetAll Queryable
        /// <summary>
        /// GetAll Queryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAllAsQueryable(
        Expression<Func<T, bool>> filter = null,
        params Expression<Func<T, object>>[] includes)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            return query;
        }
        #endregion

        #region Method :: Sum
        /// <summary>
        /// Sum
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector,
            Expression<Func<T, bool>> filter = null)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.SumAsync(selector);
        }
        #endregion

        #region Method :: Update
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
        #endregion

        #region Method :: Update Range
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<List<T>> UpdateRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();

            return entities.ToList();
        }
        #endregion
    }
}
