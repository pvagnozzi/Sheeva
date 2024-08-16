namespace Sheeva.Core;

public abstract class ExceptionBase(string? message, string? correlationId = null, Exception? innerException = null)
    : Exception(message, innerException)
{
    public string? CorrelationId { get; } = correlationId;
}
