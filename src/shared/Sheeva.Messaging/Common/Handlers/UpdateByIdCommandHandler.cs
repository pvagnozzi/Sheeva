namespace Sheeva.Messaging.Common.Handlers;

using MediatR;
using Microsoft.Extensions.Logging;
using Core;
using Sheeva.Data.Abstractions;
using Abstractions;
using Commands;

public class UpdateByIdCommandHandler<TCommand, TKey, TEntity>(IRequestHandlerContext context, ILogger logger)
    : UnitOfWorkCommandHandler<TCommand, TKey, TEntity>(context, logger)
    where TCommand : UpdateByIdCommand<TKey>, IRequest
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    protected override async Task HandleCommandAsync(IRepository<TKey, TEntity> repository, TCommand command,
        CancellationToken cancellationToken)
    {
        this.Logger.LogTrace("Find {Entity}/{Id}", typeof(TEntity).Name, command.Id);
        var item = await repository.GetByIdAsync(command.Id, cancellationToken);
        if (item is null)
        {
            this.Logger.LogError("{Entity}/{Id} not found", typeof(TEntity).Name, command.Id);
            throw new NotFoundDomainException($"{typeof(TEntity).Name}/{command.Id} not found");
        }

        item = this.MapCommandToEntity(item, command);
        await repository.UpdateAsync(item, cancellationToken);
    }

    protected virtual TEntity MapCommandToEntity(TEntity entity, TCommand command ) => this.Context.Mapper.Map<TCommand, TEntity>(command);
}
