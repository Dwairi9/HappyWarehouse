using System.Linq.Expressions;

namespace HappyWarehouse.BusinessLogic.Interfaces.IRepsitories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAllAsQueryable(
        Expression<Func<T, bool>> filter = null,
        params Expression<Func<T, object>>[] includes);

        Task<T> FirstOrDefaultAsync(
        Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includes);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<decimal> SumAsync(Expression<Func<T, decimal>> selector,
            Expression<Func<T, bool>> filter = null);

        Task<T> GetAsync(int id);

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<List<T>> UpdateRangeAsync(IEnumerable<T> entities);

        Task<T> Delete(int id);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task RemoveAsync(T entity);
        Task RemoveRange(IEnumerable<T> entities);
    }
}
