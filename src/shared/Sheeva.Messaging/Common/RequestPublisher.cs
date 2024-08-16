namespace Sheeva.Messaging.Common;

using MediatR;
using Microsoft.Extensions.Logging;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Queries;

public class RequestPublisher(IMediator mediator, ILogger<RequestPublisher> logger) : IRequestPublisher
{
    private IMediator Mediator { get; } = mediator;

    private ILogger Logger { get; } = logger;

    public async Task<TResult> SendAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>
    {
        try
        {
            this.Logger.LogTrace("Sending query {Query}", query);
            var result = await this.Mediator.Send((IRequest<TResult>) query, cancellationToken);
            this.Logger.LogTrace("Sending query {Query}: {Result}", query, result);
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Sending query {Query}: {Error}", query, ex.Message);
            throw;
        }
    }

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        try
        {
            this.Logger.LogTrace("Sending command {Command}", command);
            await this.Mediator.Send(command, cancellationToken);
            this.Logger.LogTrace("Sending command {Command}: ok", command);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Sending command {Command}: {Error}", command, ex.Message);
        }
    }
}
