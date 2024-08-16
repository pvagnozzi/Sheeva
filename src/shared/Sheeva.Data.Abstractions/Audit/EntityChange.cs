namespace Sheeva.Data.Abstractions.Audit;

public record EntityChange(
    string EntityName,
    object EntityId,
    ChangeAction Operation,
    PropertyChange[] Properties)
{
    public DateTimeOffset TimeStamp { get; init; } = DateTimeOffset.UtcNow;
}
