namespace ECommerce.Infrastructure.Caching;

public interface ICachedAggregateStore<T> where T : class
{
    Task<T?> GetAsync(string key, CancellationToken ct = default);
    Task<T> GetOrCreateAsync(
        string key,
        Func<CancellationToken, Task<T>> factory,
        CancellationToken ct = default);

    Task SetAsync(string key, T value, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);

}
