using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using System.Collections.Concurrent;

namespace ECommerce.Infrastructure.Repositories;

public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
{
    private readonly ConcurrentDictionary<Type, object> _repositories = new();
    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var type = typeof(T);
        if (_repositories.TryGetValue(type, out var repository)) return (IRepository<T>)repository;

        var newRepo = new Repository<T>(dbContext);
        _repositories.TryAdd(type, newRepo);
        return newRepo;
        
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await dbContext.SaveChangesAsync(ct);

}
