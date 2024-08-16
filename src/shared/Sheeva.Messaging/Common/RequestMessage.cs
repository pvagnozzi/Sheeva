namespace Sheeva.Messaging.Common;

using Abstractions;

public abstract record RequestMessage : IRequestMessage
{
    protected RequestMessage(IApplicationContext? context = null, string? correlationId = null)
    {
        this.Context = context ?? new ApplicationContext();
        this.CorrelationId = correlationId ?? Guid.NewGuid().ToString();
    }

    public IApplicationContext Context { get; init; }

    public string CorrelationId { get; init; }
}
