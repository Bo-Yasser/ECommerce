using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Caching;
using ECommerce.UseCases.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Extensions;

public static class CachingExtensions
{
    public static IServiceCollection AddHybridCacheWithEntitiesCaching(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.AddOptions<CacheEntryPolicy>("Basket")
                .BindConfiguration("CachedAggregate:Basket")
                .ValidateOnStart();

        services.AddSingleton<IValidateOptions<CacheEntryPolicy>, CacheEntryPolicyValidator>();

        var redisConnection = config.GetConnectionString("Redis");
        if (!string.IsNullOrWhiteSpace(redisConnection))
        {
            services.AddStackExchangeRedisCache(options => options.Configuration = redisConnection);
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        services.AddHybridCache();


        services.AddSingleton(typeof(ICachedAggregateStore<>), typeof(CachedAggregateStore<>));

        services.AddScoped<IBasketStore, HybridBasketStore>();

        return services;
    }
}