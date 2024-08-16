namespace Sheeva.Data.Abstractions.Audit;

public record PropertyChange(string PropertyName, string? OldValue = null, string? NewValue = null);
