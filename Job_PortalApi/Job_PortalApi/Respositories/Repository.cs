using Job_PortalApi.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System.Linq.Expressions;

namespace Job_PortalApi.Respositories
{
    public class Repository<TEntity>:IRepository<TEntity> where TEntity : class

    {
        private readonly JobportalContext _Context;

        public Repository(JobportalContext jobportalContext)
        {
            _Context = jobportalContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _Context.Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _Context.Set<TEntity>().Remove(entity);
            await _Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {

            return await _Context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _Context.Set<TEntity>().FindAsync(id);
        }

        public virtual IQueryable<TEntity> Table => _Context.Set<TEntity>();

        public async Task<TEntity> GetEntityByConditionAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }
        public async Task<IList<TEntity>> GetAllEntitiesAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _Context.Set<TEntity>().Where(predicate).ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _Context.Update(entity);
            await SaveChangesAsync();
        }
    }
}
