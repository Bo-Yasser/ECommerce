using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Entities.StockAggregate;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence.DbContexts;

public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(StoreDbContext).Assembly,
            type => type.Namespace == "ECommerce.Infrastructure.Persistence.Configurations");
        base.OnModelCreating(modelBuilder);
    }


    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductBrand> Brands => Set<ProductBrand>();
    public DbSet<ProductType> Types => Set<ProductType>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
    public DbSet<DeliveryMethod> DeliveryMethods => Set<DeliveryMethod>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Stock> Stocks => Set<Stock>();
    public DbSet<StockTransaction> StockTransactions => Set<StockTransaction>();
}
