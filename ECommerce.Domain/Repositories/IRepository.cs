using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
