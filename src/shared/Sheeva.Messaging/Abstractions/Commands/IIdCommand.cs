namespace Sheeva.Messaging.Abstractions.Commands;

public interface IIdCommand<out TKey> : ICommand
{
    TKey Id { get; }
}
