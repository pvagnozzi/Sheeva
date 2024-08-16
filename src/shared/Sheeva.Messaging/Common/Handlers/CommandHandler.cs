namespace Sheeva.Messaging.Common.Handlers;

using Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Abstractions;
using Commands;
using Common;

public abstract class CommandHandler<TCommand>(IRequestHandlerContext context, ILogger logger)
    : RequestHandler(context, logger), IRequestHandler<TCommand> where TCommand : Command, IRequest
{
    private IRequestNotificationService? NotificationService => this.Context.NotificationService;

    public virtual async Task Handle(TCommand request, CancellationToken cancellationToken)
    {
        var typeName = this.GetType().FullName;
        try
        {
            this.Logger.LogTrace("Command handler {TypeName}/{Request}", typeName, request);
            await this.HandleCommandAsync(request, cancellationToken);
            this.Logger.LogTrace("Command handler {TypeName}/{Request}: completed successful", typeName, request);
            await this.NotifyCompletedAsync(request, null, cancellationToken);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Command handler {TypeName}/{Request}: {Message}", typeName, request, ex.Message);
            await this.NotifyErrorAsync(request, ex, cancellationToken);
            throw new DomainException(ex.Message, request.CorrelationId, ex);
        }
    }

    protected abstract Task HandleCommandAsync(TCommand command, CancellationToken cancellationToken);

    private static RequestNotification BuildNotification(Command command, string? message = null, Exception? exception = null, bool? error = null) =>
        new(command, message, exception, error);

    private Task SaveNotificationAsync(TCommand command, bool error, string? message = null,
        CancellationToken cancellationToken = default)
    {
        if (this.NotificationService is null)
        {
            return Task.CompletedTask;
        }

        var notification = BuildNotification(command, message);
        return this.NotificationService.SaveNotificationAsync(notification, cancellationToken);
    }

    private Task NotifyCompletedAsync(TCommand command, string? message, CancellationToken cancellationToken = default) =>
        this.SaveNotificationAsync(command, false, message, cancellationToken);

    private Task NotifyErrorAsync(TCommand command, Exception ex, CancellationToken cancellationToken = default) =>
        this.SaveNotificationAsync(command, true, ex.Message, cancellationToken);
}
