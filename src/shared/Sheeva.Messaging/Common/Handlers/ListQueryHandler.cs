namespace Sheeva.Messaging.Common.Handlers;

using Microsoft.Extensions.Logging;
using Sheeva.Data.Abstractions;
using Sheeva.Data.Abstractions.Specifications;
using Abstractions;
using Queries;

public class ListQueryHandler<TQuery, TKey, TEntity>(IRequestHandlerContext context, ILogger logger)
    : UnitOfWorkQueryHandler<TQuery, IList<TEntity>, TKey, TEntity>(context, logger)
    where TQuery : ListQuery<TEntity>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected virtual Specification<TKey, TEntity> BuildSpecification(TQuery query) => new();

    protected override async Task<IList<TEntity>> HandleQueryAsync(IReadOnlyRepository<TKey, TEntity> repository,
        TQuery query,
        CancellationToken cancellationToken)
    {
        var specification = this.BuildSpecification(query);
        return await repository.ListAsync(specification, cancellationToken);
    }
}
