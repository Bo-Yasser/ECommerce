using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.UseCases.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.Infrastructure.Repositories;

public class UnitOfWork(StoreDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await dbContext.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_currentTransaction is not null) return;
        _currentTransaction = await dbContext.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            await SaveChangesAsync(ct);
            if (_currentTransaction is not null)
                await _currentTransaction.CommitAsync(ct);
        }
        catch(DbUpdateConcurrencyException ex)
        {
            await RollbackTransactionAsync(ct);
            throw new ConcurrencyConflictException("A concurrency conflict occurred while saving changes.", ex);
        }
        catch
        {
            await RollbackTransactionAsync(ct);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        try
        {
            if(_currentTransaction is not null)
                await _currentTransaction.RollbackAsync(ct);
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async ValueTask DisposeTransactionAsync()
    {
        if (_currentTransaction is not null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
