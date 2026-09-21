using ECommerce.Domain.Entities;
using ECommerce.UseCases.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.Infrastructure.Interceptors;

public class SoftDeleteInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ApplySoftDelete(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    private void ApplySoftDelete(DbContext? dbContext)
    {
        if (dbContext is null) return;

        var entries = dbContext.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Deleted);
        var userId = currentUserService.UserId;

        foreach (var entry in entries)
        {
            entry.Entity.MarkAsDeleted();
            entry.Property(e => e.DeletedById).CurrentValue = userId;
            entry.State = EntityState.Modified;
        }
    }
}
