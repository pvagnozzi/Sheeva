namespace Sheeva.Data.Abstractions;

using Core;
using Specifications;

public interface IReadOnlyRepository<TKey, TEntity> : IDisposable
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    IQueryable<TEntity> AsQuery();

    #region Async Methods
    Task<TEntity?> GetByIdAsync(TKey key, CancellationToken cancellationToken = default);

    Task<TEntity?> FirstOrDefaultAsync(ISpecification<TKey, TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<TEntity?> SingleOrDefaultAsync(ISpecification<TKey, TEntity> specification,
        CancellationToken cancellationToken = default);

    Task<IList<TEntity>> ListAsync(ISpecification<TKey, TEntity>? specification = null,
        CancellationToken cancellationToken = default);

    Task<IPagedList<TEntity>> ListAsync(IPagedSpecification<TKey, TEntity> specification,
        CancellationToken cancellationToken = default);
    #endregion

    #region Sync Methods
    TEntity? GetById(TKey key);

    TEntity? FirstOrDefault(ISpecification<TKey, TEntity> specification);

    TEntity? SingleOrDefault(ISpecification<TKey, TEntity> specification);

    IList<TEntity> List(ISpecification<TKey, TEntity>? specification = null);

    IPagedList<TEntity> List(IPagedSpecification<TKey, TEntity> specification);
    #endregion
}
