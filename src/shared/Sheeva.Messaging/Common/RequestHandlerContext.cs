namespace Sheeva.Messaging.Common;

using Sheeva.Data.Abstractions;
using Mapping;
using Abstractions;

public record RequestHandlerContext(
    IMapper Mapper,
    IUnitOfWork UnitOfWork,
    IRequestNotificationService? NotificationService = null) : IRequestHandlerContext;
