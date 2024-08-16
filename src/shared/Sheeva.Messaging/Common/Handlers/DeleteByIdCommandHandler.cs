namespace Sheeva.Messaging.Common.Handlers;

using MediatR;
using Microsoft.Extensions.Logging;
using Sheeva.Data.Abstractions;
using Abstractions;
using Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public class DeleteByIdCommandHandler<TCommand, TKey, TEntity>(IRequestHandlerContext context, ILogger logger)
    : UnitOfWorkCommandHandler<TCommand, TKey, TEntity>(context, logger)
    where TCommand : DeleteByIdCommand<TKey>, IRequest
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    protected override Task HandleCommandAsync(IRepository<TKey, TEntity> repository, TCommand command,
        CancellationToken cancellationToken) =>
        repository.DeleteByIdAsync(command.Id, cancellationToken);
}
