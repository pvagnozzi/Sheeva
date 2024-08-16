namespace Sheeva.Data.EFCore;

using System.ComponentModel.DataAnnotations;

// ReSharper disable once InconsistentNaming
public class EFStringEntity() : EFBaseEntity<string>
{
    [Key] [Required] [MaxLength(64)] public override string Id { get; init; } = Guid.NewGuid().ToString();
}
