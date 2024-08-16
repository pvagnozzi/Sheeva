namespace Sheeva.Messaging.Common.Commands;

using Abstractions;

public abstract record CreateCommand : Command
{
    protected CreateCommand(IApplicationContext? context = null, string? correlationId = null) : base(context,
        correlationId)
    {
    }
}
