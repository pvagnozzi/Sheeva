namespace Sheeva.Data.Abstractions;

using Services;

public interface IDataService : IService
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null,
        CancellationToken cancellationToken = default);

    Task<int> ExecuteAsync(string sql, object? parameters = null, CancellationToken cancellationToken = default);
}
