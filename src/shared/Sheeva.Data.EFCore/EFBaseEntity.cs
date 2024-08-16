namespace Sheeva.Data.EFCore;

using System.ComponentModel.DataAnnotations;
using Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// ReSharper disable once InconsistentNaming
public abstract class EFBaseEntity<TKey> : IEntity<TKey>
{
    [Key]
    public abstract TKey Id { get; init; }

    public DateTimeOffset CreatedOn { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedOn { get; init; } = DateTimeOffset.UtcNow;

    public bool Equals(IEntity<TKey>? other)
    {
        if (other is null)
        {
            return false;
        }

        return this.Id?.Equals(other.Id) ?? false;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return obj is IEntity<TKey> entity && this.Equals(entity);
    }

    public override int GetHashCode() => this.Id?.GetHashCode() ?? -1;
}

// ReSharper disable once InconsistentNaming
public static class EFBaseEntityExtensions
{
    public static ModelBuilder Configure<TKey, TEntity>(this ModelBuilder modelBuilder, string? tableName = null,
        Action<EntityTypeBuilder<TEntity>>? customAction = null)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        tableName ??= typeof(TEntity).Name;
        modelBuilder.Entity<TEntity>(tb =>
        {
            tb.ToTable(tableName);
            tb.HasKey(e => e.Id);
            tb.Property(e => e.Id)
                .HasMaxLength(64);
            tb.Property(e => e.CreatedOn)
                .IsRequired();
            tb.Property(e => e.UpdatedOn)
                .IsRequired();
            customAction?.Invoke(tb);
        });

        return modelBuilder;
    }
}
