namespace Sheeva.Messaging.Common.Handlers;

using Microsoft.Extensions.Logging;
using Sheeva.Data.Abstractions;
using Abstractions;
using Queries;

public class GetByIdQueryHandler<TQuery, TKey, TEntity>(IRequestHandlerContext context, ILogger logger)
    : UnitOfWorkQueryHandler<TQuery, TEntity?, TKey, TEntity>(context, logger)
    where TQuery : GetByIdQuery<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    protected override async Task<TEntity?> HandleQueryAsync(IReadOnlyRepository<TKey, TEntity> repository,
        TQuery query, CancellationToken cancellationToken) => await repository.GetByIdAsync(query.Id, cancellationToken);

}
