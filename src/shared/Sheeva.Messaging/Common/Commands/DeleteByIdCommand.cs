namespace Sheeva.Messaging.Common.Commands;

using Abstractions;
using Sheeva.Messaging.Abstractions.Commands;

public abstract record DeleteByIdCommand<TKey> : IdCommand<TKey>, IDeleteCommand<TKey>
{
    protected DeleteByIdCommand(TKey id, IApplicationContext? context = null, string? correlationId = null) : base(id,
        context, correlationId)
    {
    }
}
