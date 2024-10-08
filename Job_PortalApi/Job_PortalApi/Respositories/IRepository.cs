using Job_PortalApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace Job_PortalApi.Respositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);

        Task<IList<T>> GetAllEntitiesAsync(Expression<Func<T, bool>> predicate);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T  entity);
        Task SaveChangesAsync();
        Task<T> GetEntityByConditionAsync(Expression<Func<T, bool>> predicate);

    }
}
