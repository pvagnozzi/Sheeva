namespace Sheeva.Messaging.Common.Handlers;

using Microsoft.Extensions.Logging;
using Sheeva.Data.Abstractions;
using Abstractions;
using Queries;

public abstract class UnitOfWorkQueryHandler<TQuery, TResponse, TKey, TEntity>(IRequestHandlerContext context, ILogger logger)
    : QueryHandler<TQuery, TResponse>(context, logger)
    where TQuery : Query<TResponse>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private IUnitOfWork UnitOfWork => this.Context.UnitOfWork;

    protected override async Task<TResponse> HandleQueryAsync(TQuery query,
        CancellationToken cancellationToken)
    {
        using var repository = this.UnitOfWork.GetReadOnlyRepository<TKey, TEntity>();
        return await this.HandleQueryAsync(repository, query, cancellationToken);
    }

    protected abstract Task<TResponse> HandleQueryAsync(IReadOnlyRepository<TKey, TEntity> repository, TQuery query,
        CancellationToken cancellationToken);

    protected override void DisposeResources()
    {
        this.UnitOfWork.Dispose();
        base.DisposeResources();
    }
}
