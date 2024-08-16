namespace Sheeva.Data.Abstractions;

public interface IEntity<TKey> : IEquatable<IEntity<TKey>>
{
    TKey Id { get; }

    DateTimeOffset CreatedOn { get; }

    DateTimeOffset UpdatedOn { get; }
}
