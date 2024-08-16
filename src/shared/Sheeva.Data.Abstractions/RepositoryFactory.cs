namespace Sheeva.Data.Abstractions;

using Microsoft.Extensions.Logging;

public abstract class RepositoryFactory(IRepositoryMapper repositoryMapper, ILoggerFactory loggerFactory)
    : IRepositoryFactory
{
    private ILoggerFactory LoggerFactory { get; } = loggerFactory;

    public IRepositoryMapper RepositoryMapper { get; } = repositoryMapper;

    public IReadOnlyRepository<TKey, TEntity> CreateReadOnlyRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        var type = this.RepositoryMapper.GetRepositoryType(typeof(TEntity));
        var logger = this.LoggerFactory.CreateLogger(typeof(IReadOnlyRepository<TKey, TEntity>));

        var result = type is not null
            ? (IReadOnlyRepository<TKey, TEntity>?)this.CreateInstance(type, logger)
            : null;
        return result ?? this.CreateReadOnlyRepositoryInstance<TKey, TEntity>(logger);
    }

    public virtual IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        var type = this.RepositoryMapper.GetRepositoryType(typeof(TEntity));
        var logger = this.LoggerFactory.CreateLogger(typeof(IReadOnlyRepository<TKey, TEntity>));

        var result = type is not null
            ? (IRepository<TKey, TEntity>?)this.CreateInstance(type, logger)
            : null;
        return result ?? this.CreateRepositoryInstance<TKey, TEntity>(logger);
    }

    protected abstract object? CreateInstance(Type type, ILogger logger);

    protected abstract IReadOnlyRepository<TKey, TEntity> CreateReadOnlyRepositoryInstance<TKey, TEntity>(
        ILogger logger)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>;

    protected abstract IRepository<TKey, TEntity> CreateRepositoryInstance<TKey, TEntity>(ILogger logger)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>;
}
