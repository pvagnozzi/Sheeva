namespace Sheeva.Data.Abstractions;

using Microsoft.Extensions.Logging;
using Core;

public abstract class UnitOfWork : Disposable, IUnitOfWork
{
    private readonly ILoggerFactory loggerFactory;

    private readonly IRepositoryMapper repositoryMapper;

    private IRepositoryFactory? repositoryFactory;

    protected UnitOfWork(IRepositoryMapper repositoryMapper, ILoggerFactory loggerFactory)
    {
        this.repositoryMapper = repositoryMapper;
        this.loggerFactory = loggerFactory;
        this.Logger = this.loggerFactory.CreateLogger<UnitOfWork>();
    }

    protected ILogger Logger { get; }

    private IRepositoryFactory RepositoryFactory =>
        this.repositoryFactory ??= this.BuildRepositoryFactory(this.repositoryMapper, this.loggerFactory);

    public IReadOnlyRepository<TKey, TEntity> GetReadOnlyRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        this.Logger.LogDebug("GetReadOnlyRepository '{Entity}'", typeof(TEntity));
        var result = this.RepositoryFactory.CreateReadOnlyRepository<TKey, TEntity>();
        return result;
    }

    public IRepository<TKey, TEntity> GetRepository<TKey, TEntity>()
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        this.Logger.LogDebug("GetRepository '{Entity}'", typeof(TEntity));
        var result = this.RepositoryFactory.CreateRepository<TKey, TEntity>();
        return result;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.Logger.LogDebug("SaveChangesAsync");
        await this.SaveUnitOfWorkAsync(cancellationToken);
        this.Logger.LogDebug("SaveChangesAsync: completed");
    }

    public void SaveChanges()
    {
        this.Logger.LogDebug("SaveChanges");
        this.SaveUnitOfWork();
        this.Logger.LogDebug("SaveChanges: completed");
    }

    protected override void DisposeResources()
    {
        this.Logger.LogDebug("DisposeResources");
        this.DisposeUnitOfWork();
        base.DisposeResources();
    }

    protected abstract IRepositoryFactory BuildRepositoryFactory(IRepositoryMapper repositoryMapper,
        ILoggerFactory loggerFactory);

    protected abstract void DisposeUnitOfWork();

    protected abstract Task SaveUnitOfWorkAsync(CancellationToken cancellationToken = default);

    protected abstract void SaveUnitOfWork();
}
