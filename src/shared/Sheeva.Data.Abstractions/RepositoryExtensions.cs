namespace Sheeva.Data.Abstractions;

using System.Linq.Expressions;
using Core;
using Specifications;

public static class RepositoryExtensions
{
    public static Task<IList<TEntity>> ListAsync<TKey, TEntity>(this IReadOnlyRepository<TKey, TEntity> repository,
        Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        where TKey : IEquatable<TKey> where TEntity : class, IEntity<TKey> =>
        repository.ListAsync(new Specification<TKey, TEntity>(expression), cancellationToken);

    public static Task<IPagedList<TEntity>> ListAsync<TKey, TEntity>(this IReadOnlyRepository<TKey, TEntity> repository,
        Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        where TKey : IEquatable<TKey> where TEntity : class, IEntity<TKey> =>
        repository.ListAsync(new PagedSpecification<TKey, TEntity>(expression, pageIndex: pageIndex, pageSize: pageSize), cancellationToken);
}
