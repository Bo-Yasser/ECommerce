using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data.DbContexts;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }


}
