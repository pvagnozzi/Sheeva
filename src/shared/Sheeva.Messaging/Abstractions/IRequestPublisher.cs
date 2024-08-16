namespace Sheeva.Messaging.Abstractions;

using Commands;
using Queries;

public interface IRequestPublisher
{
    Task<TResult> SendAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResult>;

    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand;
}
