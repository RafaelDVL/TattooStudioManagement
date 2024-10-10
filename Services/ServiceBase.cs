using StudioTattooManagement.Interfaces.Irepositories;
using StudioTattooManagement.Interfaces.Iservices;
using System.Linq.Expressions;

namespace StudioTattooManagement.Services
{
    public class ServiceBase<T>: IServiceBase<T> where T : class
    {
        private readonly IRepositoryBase<T> _repository;

        public ServiceBase(IRepositoryBase<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.FindAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _repository.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _repository.Update(entity);
        }

        public void Remove(T entity)
        {
            _repository.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _repository.RemoveRange(entities);
        }
    }
}
