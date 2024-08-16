namespace Sheeva.Messaging.Abstractions;

public interface IRequestNotificationService : IDisposable
{
    Task SaveNotificationAsync(IRequestNotification requestNotification, CancellationToken cancellationToken = default);

    Task MarkNotificationAsync(string correlationId, CancellationToken cancellationToken = default);
}
