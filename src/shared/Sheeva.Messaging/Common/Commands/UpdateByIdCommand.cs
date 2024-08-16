namespace Sheeva.Messaging.Common.Commands;

using Abstractions;
using Sheeva.Messaging.Abstractions.Commands;

// ReSharper disable once ClassNeverInstantiated.Global
public record UpdateByIdCommand<TKey> : IdCommand<TKey>, IUpdateCommand<TKey>
{
    public UpdateByIdCommand(TKey id, IApplicationContext? context = null, string? correlationId = null) : base(id, context, correlationId)
    {
    }
}
