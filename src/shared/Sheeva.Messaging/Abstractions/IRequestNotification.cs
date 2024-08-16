namespace Sheeva.Messaging.Abstractions;

public interface IRequestNotification
{
    IRequestMessage Request { get; }

    Exception? Exception { get; }

    bool Error { get; }

    string? Message { get; }

    DateTimeOffset TimeStamp { get; }

    DateTimeOffset? MarkTimeStamp { get; }
}
