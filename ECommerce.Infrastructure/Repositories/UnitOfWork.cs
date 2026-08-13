using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using System.Collections.Concurrent;

namespace ECommerce.Infrastructure.Repositories;

public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await dbContext.SaveChangesAsync(ct);
}
