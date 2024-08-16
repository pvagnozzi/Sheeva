namespace Sheeva.Messaging.Abstractions.Commands;

public interface IDeleteCommand<out TKey> : IIdCommand<TKey>;
