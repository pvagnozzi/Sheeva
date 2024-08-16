namespace Sheeva.Cache;

public interface ICache : IDisposable
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T: class;

    Task<T> GetOrCreateAsync<T>(string key, T defaultValue, CancellationToken cancellationToken = default) where T: class;

    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T: class;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
