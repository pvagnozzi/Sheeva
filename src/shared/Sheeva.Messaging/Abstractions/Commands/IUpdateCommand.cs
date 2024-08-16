namespace Sheeva.Messaging.Abstractions.Commands;

public interface IUpdateCommand<out TKey> : IIdCommand<TKey>;
