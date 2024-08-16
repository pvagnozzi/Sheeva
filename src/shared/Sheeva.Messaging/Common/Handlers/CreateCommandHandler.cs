namespace Sheeva.Messaging.Common.Handlers;

using MediatR;
using Microsoft.Extensions.Logging;
using Sheeva.Data.Abstractions;
using Abstractions;
using Commands;

public class CreateCommandHandler<TCommand, TKey, TEntity>(IRequestHandlerContext context, ILogger logger) :
    UnitOfWorkCommandHandler<TCommand, TKey, TEntity>(context, logger)
    where TCommand : CreateCommand, IRequest
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    protected override async Task HandleCommandAsync(IRepository<TKey, TEntity> repository, TCommand command,
        CancellationToken cancellationToken)
    {
        var item = this.MapCommandToEntity(command);
        await repository.AddAsync(item, cancellationToken);
    }

    protected virtual TEntity MapCommandToEntity(TCommand command) => this.Context.Mapper.Map<TCommand, TEntity>(command);
}
