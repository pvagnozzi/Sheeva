namespace Sheeva.Messaging.Common.Handlers;

using Microsoft.Extensions.Logging;
using Core;
using Sheeva.Data.Abstractions;
using Sheeva.Data.Abstractions.Specifications;
using Abstractions;
using Queries;

public class PagedListQueryHandler<TQuery, TKey, TEntity>(IRequestHandlerContext context, ILogger logger)
    : UnitOfWorkQueryHandler<TQuery, IPagedList<TEntity>, TKey, TEntity>(context, logger)
    where TQuery : PagedListQuery<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected virtual PagedSpecification<TKey, TEntity> BuildSpecification(TQuery query) => new();

    protected override Task<IPagedList<TEntity>> HandleQueryAsync(IReadOnlyRepository<TKey, TEntity> repository,
        TQuery query, CancellationToken cancellationToken)
    {
        var specification = this.BuildSpecification(query);
        return repository.ListAsync(specification, cancellationToken);
    }
}
