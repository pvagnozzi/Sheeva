namespace Sheeva.Core;

public class NotFoundDomainException(string message, string? correlationId = null, Exception? innerException = null)
    : DomainException(message, correlationId, innerException);
