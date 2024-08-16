namespace Sheeva.Messaging.Abstractions;

public interface IApplicationContext
{
    IAuthenticationInfo? Authentication { get; }
}
