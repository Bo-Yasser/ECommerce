using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;

namespace ECommerce.Infrastructure.Caching;

public class HybridBasketStore(ICachedAggregateStore<Basket> store) : IBasketStore
{
    public async Task<Basket> GetOrCreateAsync(Guid buyerId, CancellationToken ct = default)
        => await store.GetOrCreateAsync(
            BuildCacheKey(buyerId),
            async cancel =>
            {
                var createResult = Basket.CreateEmpty(buyerId);
                if (createResult.IsFailure) throw new InvalidOperationException(createResult.Error!.Message);
                return createResult.Value;
            },
            ct);
    public async Task<Basket?> GetAsync(Guid buyerId, CancellationToken ct = default)
        => await store.GetAsync(BuildCacheKey(buyerId), ct);

    public async Task SaveAsync(Basket basket, CancellationToken ct = default)
        => await store.SetAsync(BuildCacheKey(basket.BuyerId), basket, ct);

    public async Task DeleteAsync(Guid buyerId, CancellationToken ct = default)
        => await store.RemoveAsync(BuildCacheKey(buyerId), ct);

    private static string BuildCacheKey(Guid buyerId) => $"basket: {buyerId}";

}
