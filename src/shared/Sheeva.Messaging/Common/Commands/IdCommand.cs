namespace Sheeva.Messaging.Common.Commands;

using Abstractions;
using Sheeva.Messaging.Abstractions.Commands;

public abstract record IdCommand<TKey> : Command, IIdCommand<TKey>
{
    protected IdCommand(TKey id, IApplicationContext? context = null, string? correlationId = null) : base(context,
        correlationId) =>
        this.Id = id;

    public TKey Id { get; init; }
}
