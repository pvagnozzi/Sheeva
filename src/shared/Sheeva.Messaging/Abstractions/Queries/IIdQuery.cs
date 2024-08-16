namespace Sheeva.Messaging.Abstractions.Queries;

public interface IIdQuery<TResult> : IQuery<TResult>
{
    string Id { get; }
}
