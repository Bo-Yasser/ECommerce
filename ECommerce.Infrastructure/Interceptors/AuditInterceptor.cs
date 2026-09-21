using ECommerce.Domain.Entities;
using ECommerce.UseCases.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.Infrastructure.Interceptors
{
    public class AuditInterceptor(ICurrentUserService currentUserService) : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        private void ApplyAudit(DbContext? dbContext)
        {
            if (dbContext is null) return;

            var entries = dbContext.ChangeTracker.Entries<BaseEntity>();
            var userId = currentUserService.UserId;
            var utcNow = DateTimeOffset.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(e => e.CreatedAt).CurrentValue = utcNow;
                    entry.Property(e => e.CreatedById).CurrentValue = userId;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.UpdatedAt).CurrentValue = utcNow;
                    entry.Property(e => e.UpdatedById).CurrentValue = userId;
                }
            }
        }

    }
}
