namespace Sheeva.Messaging.Common.Commands;

using MediatR;
using Abstractions;
using Sheeva.Messaging.Abstractions.Commands;
using Common;

public abstract record Command : RequestMessage, ICommand, IRequest
{
    protected Command(IApplicationContext? context = null, string? correlationId = null) : base(context, correlationId)
    {
    }
}
