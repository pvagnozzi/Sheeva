namespace Sheeva.Cache;

using Core;
using Microsoft.Extensions.Caching.Memory;

public class LocalCache(MemoryCache memoryCache) : Disposable, ICache
{
    public LocalCache(MemoryCacheOptions memoryCacheOptions) : this(new MemoryCache(memoryCacheOptions))
    {
    }

    public LocalCache() : this(new MemoryCacheOptions())
    {
    }

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T: class
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(memoryCache.Get<T>(key));
    }

    public async Task<T> GetOrCreateAsync<T>(string key, T defaultValue, CancellationToken cancellationToken = default) where T: class
    {
        cancellationToken.ThrowIfCancellationRequested();
        return (await memoryCache.GetOrCreateAsync<T>(key, _ => Task.FromResult(defaultValue))) ?? defaultValue;
    }

    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T: class
    {
        cancellationToken.ThrowIfCancellationRequested();
        memoryCache.Set(key, value);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        memoryCache.Remove(key);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(memoryCache.TryGetValue(key, out var _));
    }
}
