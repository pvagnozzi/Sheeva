namespace Sheeva.Data.Abstractions.Audit;

public interface IPropertyAudit
{
    string PropertyName { get; }

    string? OldValue { get; }

    string? NewValue { get; }
}
