namespace Sheeva.Cache;

using Microsoft.Extensions.Caching.Distributed;
using Core;

public class RemoteCache(IDistributedCache distributedCache) : Disposable, ICache
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T: class
    {
        var data = await distributedCache.GetAsync(key, cancellationToken);
        return data is not null ? Deserialize<T>(data) : default;
    }

    public async Task<T> GetOrCreateAsync<T>(string key, T defaultValue, CancellationToken cancellationToken = default) where T: class
    {
        var data = await distributedCache.GetAsync(key, cancellationToken);
        if (data is not null)
        {
            return Deserialize<T>(data);
        }

        await this.SetAsync(key, defaultValue, cancellationToken);
        return defaultValue;
    }

    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T: class
    {
        var data = Serialize(value);
        return distributedCache.SetAsync(key, data, new DistributedCacheEntryOptions(), cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default) =>
        distributedCache.RefreshAsync(key, cancellationToken);

    private static T Deserialize<T>(byte[] data) =>
        (T)Convert.ChangeType(data, typeof(T));

    private static byte[] Serialize<T>(T data) where T: class =>
        (byte[])Convert.ChangeType(data, typeof(byte[]));
}
