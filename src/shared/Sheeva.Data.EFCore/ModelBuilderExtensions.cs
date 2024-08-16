namespace Sheeva.Data.EFCore;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class ModelBuilderExtensions
{

    public static EntityTypeBuilder<TEntity> SetStringBaseEntity<TEntity>(
        this EntityTypeBuilder<TEntity> builder)
        where TEntity : EFBaseEntity<string> => SetBaseEntity<string, TEntity>(builder);

    private static EntityTypeBuilder<TEntity> SetBaseEntity<TKey, TEntity>(
        this EntityTypeBuilder<TEntity> builder)
        where TEntity : EFBaseEntity<TKey>
    {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .HasMaxLength(64)
            .IsRequired();
        builder
            .Property(x => x.CreatedOn)
            .IsRequired();
        builder
            .Property(x => x.UpdatedOn)
            .IsRequired();
        return builder;
    }

}
