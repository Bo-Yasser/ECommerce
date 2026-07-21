using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public sealed class Repository<T>(StoreDbContext _dbContext) : IRepository<T> where T : BaseEntity
{
    private readonly DbSet<T> _dbSet = _dbContext.Set<T>();
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbSet
            .FirstOrDefaultAsync(entity => entity.Id == id, ct);
    }
    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct)
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }
    public void Update(T entity)
    {
        _dbSet .Update(entity);
    }
    public void Delete(T entity)
    {
        entity.MarkAsDeleted();
    }
}
