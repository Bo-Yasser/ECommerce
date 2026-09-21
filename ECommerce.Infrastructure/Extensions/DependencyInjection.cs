using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Interceptors;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<AuditInterceptor>();

        services.AddDbContext<StoreDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__ApplicationMigrationsHistory"))
                    .EnableSensitiveDataLogging()
                    .AddInterceptors(
                        sp.GetRequiredService<SoftDeleteInterceptor>(),
                        sp.GetRequiredService<AuditInterceptor>()
                    );

        });

        services.AddDbContext<IdentityStoreDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                config.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsHistoryTable("__IdentityMigrationsHistory"))
                    .EnableSensitiveDataLogging()
                    .AddInterceptors(
                        sp.GetRequiredService<SoftDeleteInterceptor>(),
                        sp.GetRequiredService<AuditInterceptor>()
                    );
        });

        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<IDataSeeder, IdentitySeeder>();
        services.AddScoped<IDataSeeder, DeliveryMethodSeeder>();
        services.AddScoped<DatabaseSeeder>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddHybridCacheWithEntitiesCaching(config);
        services.AddAuthAndIdentityConfigurations();
        services.AddEmailConfigurations();

        return services;

    }
}
