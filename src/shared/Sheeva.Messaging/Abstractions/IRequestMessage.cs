namespace Sheeva.Messaging.Abstractions;

public interface IRequestMessage
{
    string CorrelationId { get; }
}
