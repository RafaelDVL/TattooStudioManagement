using Microsoft.EntityFrameworkCore;
using StudioTattooManagement.Data;
using StudioTattooManagement.Interfaces.Irepositories;
using System.Linq.Expressions;

namespace StudioTattooManagement.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
