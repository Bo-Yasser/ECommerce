using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Models;
using ECommerce.UseCases.Common.Options;

namespace ECommerce.Infrastructure.Caching;

public sealed class HybridPendingRegistrationStore(
    HybridCache cache,
    IOptions<EmailVerificationSettings> options) : IPendingRegistrationStore
{
    private readonly EmailVerificationSettings _settings = options.Value;

    public async Task SaveAsync(string email, PendingRegistrationPayload payload, CancellationToken ct = default)
    {
        var cacheKey = BuildCacheKey(email);

        var entryOptions = new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(_settings.ExpirationMinutes),
            LocalCacheExpiration = TimeSpan.FromMinutes(_settings.ExpirationMinutes)
        };

        await cache.SetAsync(cacheKey, payload, entryOptions, cancellationToken: ct);
    }

    public async Task<PendingRegistrationPayload?> ValidateAndConsumeAsync(string email, string code, CancellationToken ct = default)
    {
        var cacheKey = BuildCacheKey(email);

        var payload = await cache.TryGetAsync<PendingRegistrationPayload>(cacheKey, ct);

        // Check Existence / Expiration
        if (payload is null) return null;

        // Validate Code
        if (payload.VerificationCode != code)
        {
            var updatedPayload = payload with { FailedAttempts = payload.FailedAttempts + 1 };
            await SaveAsync(payload.Email, updatedPayload, ct);

            return updatedPayload;
        }

        // The Consume-Once Guarantee
        await cache.RemoveAsync(cacheKey, ct);

        return payload;
    }

    public async Task RemoveAsync(string email, CancellationToken ct = default)
    {
        var cacheKey = BuildCacheKey(email);
        await cache.RemoveAsync(cacheKey, ct);
    }

    public async Task<PendingRegistrationPayload?> GetAsync(string email, CancellationToken ct = default)
    {
        var cacheKey = BuildCacheKey(email);
        var payload = await cache.TryGetAsync<PendingRegistrationPayload>(cacheKey, ct);

        if(payload is null) return null;
        return payload;
    }
    private static string BuildCacheKey(string email)
        => $"pending-registration:{email.ToLowerInvariant()}";
}
