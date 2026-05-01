using Domain.Entities;
using Domain.Specifications;

namespace Domain.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task UpsertAsync(T entity);
        Task DeleteAsync(T entity);
    }
}