using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
