namespace Sheeva.Data.Abstractions;

public interface IUnitOfWork : IDisposable
{
    IReadOnlyRepository<TKey, TEntity> GetReadOnlyRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>;

    IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>;

    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    void SaveChanges();
}
