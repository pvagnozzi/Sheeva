namespace Sheeva.Messaging.Abstractions;

public interface IAuthenticationInfo
{
    string AuthenticationId { get; }

    string UserName { get; }

    string Email { get; }

    string? FirstName { get; }

    string? LastName { get; }

    string? FullName { get; }

    string? City { get; }

    string? Country { get; }

    string? PostalCode { get; }

    string? State { get; }

    string? StreetAddress { get; }
}
