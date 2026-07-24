using ECommerce.UseCases.ProductBrands.Queries;
using ECommerce.UseCases.Products.Queries;
using ECommerce.UseCases.Profiles;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.UseCases;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ProductConfig).Assembly);
        
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddScoped<GetAllProductsQuery>();
        services.AddScoped<GetByIdProductQuery>();
        services.AddScoped<GetAllBrandsQuery>();

        return services;
    }
}
 