namespace Sheeva.Messaging.Common.Handlers;

using Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Sheeva.Data.Abstractions;
using Abstractions;

public abstract class UnitOfWorkCommandHandler<TCommand, TKey, TEntity>(
    IRequestHandlerContext context, ILogger logger) : CommandHandler<TCommand>(context, logger)
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
    where TCommand : Command, IRequest
{
    private IUnitOfWork UnitOfWork => this.Context.UnitOfWork;

    protected override async Task HandleCommandAsync(TCommand command, CancellationToken cancellationToken)
    {
        try
        {
            using var repository = this.UnitOfWork.GetRepository<TKey, TEntity>();
            this.Logger.LogTrace("Handling command {Command}", command);
            await this.HandleCommandAsync(repository, command, cancellationToken);
            this.Logger.LogTrace("Handling command {Command}: ok", command);
            await this.UnitOfWork.SaveChangesAsync(cancellationToken);
            this.Logger.LogTrace("Handling command {Command}: changes saved", command);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Handling command {Command}: {Error}", command, ex.Message);
            throw;
        }

    }

    protected abstract Task HandleCommandAsync(IRepository<TKey, TEntity> repository, TCommand command, CancellationToken cancellationToken);

    protected override void DisposeResources()
    {
        this.UnitOfWork.Dispose();
        base.DisposeResources();
    }
}
