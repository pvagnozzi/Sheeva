namespace Sheeva.Data.EFCore.Audit;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using Sheeva.Data.Abstractions.Audit;
using EFCore;

public class EntityAudit(string entityName, string entityId, ChangeAction changeAction, string properties)
    : EFStringEntity, IEntityAudit
{
    protected EntityAudit() : this(string.Empty, string.Empty, ChangeAction.Inserted, string.Empty)
    {
    }

    [Required] [MaxLength(256)] public string EntityName { get; init; } = entityName;

    [Required] [MaxLength(64)] public string EntityId { get; init; } = entityId;

    [Required] public ChangeAction Action { get; init; } = changeAction;

    public string Properties { get; init; } = properties;

    IPropertyAudit[] IEntityAudit.Properties => this.Properties.DeserializePropertyAudit();
}

public static class EntityAuditExtensions
{
    public static EntityAudit ToEntityAudit(this EntityChange entityChange) =>
        new(
            entityChange.EntityName,
            entityChange.EntityId.ToString()!,
            entityChange.Operation,
            entityChange.Properties.Serialize());

    public static IPropertyAudit[] DeserializePropertyAudit(this string properties) =>
        JsonSerializer.Deserialize<PropertyAuditJsonSerialization[]>(properties)
            ?.Select(x => new PropertyAudit(x.PropertyName, x.OldValue, x.NewValue)).Cast<IPropertyAudit>().ToArray() ??
        [];

    private static string Serialize(this IEnumerable<PropertyChange> properties) =>
        JsonSerializer.Serialize(properties.Select(x =>
            new PropertyAuditJsonSerialization(x.PropertyName, x.OldValue?.ToString(), x.NewValue?.ToString())));

    private record PropertyAuditJsonSerialization(
        [property: JsonPropertyName("propertyName")]
        string PropertyName,
        [property: JsonPropertyName("oldValue")]
        string? OldValue,
        [property: JsonPropertyName("newValue")]
        string? NewValue);
}
