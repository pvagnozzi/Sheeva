namespace Sheeva.Data.EFCore.Audit;

using Sheeva.Data.Abstractions.Audit;

public record PropertyAudit(string PropertyName, string? OldValue, string? NewValue) : IPropertyAudit;
