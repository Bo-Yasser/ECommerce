using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Interceptors;
using ECommerce.Infrastructure.Persistence.DbContexts;
using ECommerce.Infrastructure.Persistence.Queries;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.Infrastructure.Repositories;
using ECommerce.UseCases.ProductBrands;
using ECommerce.UseCases.Products;
using ECommerce.UseCases.ProductTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

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
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
                    .EnableSensitiveDataLogging()
                    .AddInterceptors(
                        sp.GetRequiredService<SoftDeleteInterceptor>(),
                        sp.GetRequiredService<AuditInterceptor>()
                    );

        });

        services.AddScoped<IDataSeeder, ProductBrandSeeder>();
        services.AddScoped<IDataSeeder, ProductTypeSeeder>();
        services.AddScoped<DatabaseSeeder>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProductQueryService, ProductQueryService>();
        services.AddScoped<IProductBrandQueryService, ProductBrandQueryService>();
        services.AddScoped<IProductTypeQueryService, ProductTypeQueryService>();


        return services;

    }
}
