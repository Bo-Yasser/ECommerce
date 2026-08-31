using ECommerce.UseCases.Common.Options;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Caching;

public sealed class CachedAggregateStore<T>(
    HybridCache cache,
    IOptionsMonitor<CacheEntryPolicy> options) : ICachedAggregateStore<T> where T : class
{
    private readonly CacheEntryPolicy _options = options.Get(typeof(T).Name);
    public async Task<T?> GetAsync(string key, CancellationToken ct = default)
    {
        var envelope = await cache.TryGetAsync<CacheEnvelope<T>>(key, ct);
        return envelope?.Payload;
    }

    public async Task<T> GetOrCreateAsync(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CancellationToken ct = default)
    {
        var envelope = await cache.GetOrCreateAsync(
                key,
                async cancel =>
                {
                    var value = await factory(cancel);
                    var utcNow = DateTimeOffset.UtcNow;
                    return new CacheEnvelope<T> { Payload = value, CreatedAtUtc = utcNow, LastAccessedUtc = utcNow };
                },
                CreateEntryOptionsForNewEnvelope(),
                cancellationToken: ct
            );


        await RefreshExpirationIfNeedAsync(key, envelope, ct);
        return envelope.Payload;
    }


    public async Task SetAsync(string key, T value, CancellationToken ct = default)
    {
        var existing = await cache.TryGetAsync<CacheEnvelope<T>>(key, ct);

        var envelope = new CacheEnvelope<T> 
        { 
            Payload = value, 
            CreatedAtUtc = existing?.CreatedAtUtc ?? DateTimeOffset.UtcNow,
            LastAccessedUtc = DateTimeOffset.UtcNow,

        };
        await SetOrRemoveIfExpiredAsync(key, envelope, ct);
    }

    public async Task RemoveAsync(string key, CancellationToken ct = default) 
        => await cache.RemoveAsync(key, ct);



    #region Helper Methods

    private TimeSpan? CalculateExpiration(
        DateTimeOffset createdAtUtc,
        DateTimeOffset lastAccessedUtc,
        DateTimeOffset utcNow)
    {
        var absoluteRemaining = createdAtUtc
            .AddDays(_options.AbsoluteExpirationDays)
            .Subtract(utcNow);

        var slidingRemaining = lastAccessedUtc
            .AddDays(_options.SlidingExpirationDays)
            .Subtract(utcNow);

        if (absoluteRemaining <= TimeSpan.Zero || slidingRemaining <= TimeSpan.Zero)
            return null;


        return absoluteRemaining <= slidingRemaining
            ? absoluteRemaining
            : slidingRemaining;
    }
    private HybridCacheEntryOptions CreateEntryOptions(TimeSpan expiration)
    {
        var localExpiration = TimeSpan.FromMinutes(_options.LocalCacheExpirationMinutes);
        if (localExpiration > expiration)
            localExpiration = expiration;
        return new()
        {
            Expiration = expiration,
            LocalCacheExpiration = localExpiration
        };
    }
    private HybridCacheEntryOptions CreateEntryOptionsForNewEnvelope()
    {
        var utcNow = DateTimeOffset.UtcNow;

        var expiration = CalculateExpiration(utcNow, utcNow, utcNow) ?? throw new ArgumentNullException();

        return CreateEntryOptions(expiration);
    }

    private async Task RefreshExpirationIfNeedAsync(string key, CacheEnvelope<T> envelope, CancellationToken ct)
    {
        var utcNow = DateTimeOffset.UtcNow;
        var age = utcNow - envelope.LastAccessedUtc;

        if (age < TimeSpan.FromMinutes(_options.SlidingRefreshThresholdMinutes))
            return;


        var refreshedEnvelope = new CacheEnvelope<T>
        {
            Payload = envelope.Payload,
            CreatedAtUtc = envelope.CreatedAtUtc,
            LastAccessedUtc = utcNow
        };

        await SetOrRemoveIfExpiredAsync(key, refreshedEnvelope, ct);
    }

    private async Task SetOrRemoveIfExpiredAsync(string key, CacheEnvelope<T> refreshedEnvelope, CancellationToken ct)
    {
        var expiration = CalculateExpiration(
            refreshedEnvelope.CreatedAtUtc,
            refreshedEnvelope.LastAccessedUtc,
            DateTimeOffset.UtcNow);

        if(expiration is null)
        {
            await cache.RemoveAsync(key, ct);
            return;
        }

        await cache.SetAsync(
            key,
            refreshedEnvelope,
            CreateEntryOptions(expiration.Value),
            cancellationToken:ct);
    }


    #endregion
}
