namespace Sheeva.Data.Abstractions;

public interface IRepository<TKey, TEntity> : IReadOnlyRepository<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    #region Sync Methods
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task DeleteRangeByIdAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default);
    #endregion

    #region Sync Methods
    void Add(TEntity entity);

    void Update(TEntity entity);

    void Delete(TEntity entity);

    void DeleteById(TKey id);

    void AddRange(IEnumerable<TEntity> entities);

    void UpdateRange(IEnumerable<TEntity> entities);

    void DeleteRange(IEnumerable<TEntity> entities);

    void DeleteRangeById(IEnumerable<TKey> ids);
    #endregion
}
