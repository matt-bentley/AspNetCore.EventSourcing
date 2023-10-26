
namespace AspNetCore.EventSourcing.Application.Abstractions.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        IQueryable<T> GetAll(bool noTracking = true);
        Task<T?> GetByIdAsync(Guid id);
        void Insert(T entity);
        void Insert(List<T> entities);
        void Delete(T entity);
        void Remove(IEnumerable<T> entitiesToRemove);
    }
}
