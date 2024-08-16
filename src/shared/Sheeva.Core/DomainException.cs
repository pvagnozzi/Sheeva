namespace Sheeva.Core;

public class DomainException(string message, string? correlationId = null, Exception? innerException = null)
    : ExceptionBase(message, correlationId, innerException);
