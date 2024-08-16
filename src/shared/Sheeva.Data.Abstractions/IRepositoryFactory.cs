namespace Sheeva.Data.Abstractions;

public interface IRepositoryFactory
{
    IRepositoryMapper RepositoryMapper { get; }

    IReadOnlyRepository<TKey, TEntity> CreateReadOnlyRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>;

    IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>;
}
