namespace Sheeva.Messaging.Common;

using Abstractions;

public record ApplicationContext(IAuthenticationInfo? Authentication = null) : IApplicationContext;
