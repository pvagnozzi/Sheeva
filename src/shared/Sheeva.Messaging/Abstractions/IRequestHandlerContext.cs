namespace Sheeva.Messaging.Abstractions;

using Data.Abstractions;
using Mapping;

public interface IRequestHandlerContext
{
    IRequestNotificationService? NotificationService { get; }

    IMapper Mapper { get; }

    IUnitOfWork UnitOfWork { get; }
}
