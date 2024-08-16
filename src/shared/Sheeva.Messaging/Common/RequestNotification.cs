namespace Sheeva.Messaging.Common;

using Abstractions;

public record RequestNotification : IRequestNotification
{
    public RequestNotification(IRequestMessage request, string? message = null, Exception? exception = null,
        bool? error = null)
    {
        this.Request = request;
        this.Message = message ?? exception?.Message;
        this.Exception = exception;
        this.Error = error ?? exception != null;
    }

    public IRequestMessage Request { get; init; }

    public Exception? Exception { get; init; }

    public bool Error { get; init; }

    public string? Message { get; init; }

    public DateTimeOffset TimeStamp { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? MarkTimeStamp { get; protected internal set; }
}

public static class RequestNotificationExtensions
{
    public static RequestNotification SetMarked(this RequestNotification notification,
        DateTimeOffset? markedTime = null)
    {
        notification.MarkTimeStamp = markedTime ?? DateTimeOffset.UtcNow;
        return notification;
    }
}
