namespace Sheeva.Data.EFCore;

using System.Data;
using System.Transactions;
using Abstractions.Audit;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

public static class DbContextExtensions
{
    public static string GetDescription(this CommandDefinition definition) => $"[CommandType='{definition.CommandType}', CommandTimeout='{definition.CommandTimeout}']{definition.CommandText}";

    private static CommandDefinition CreateCommand(
        this DbContext context,
        string text,
        object? parameters = null,
        int? timeout = null,
        CommandType type = CommandType.Text,
        CancellationToken cancellationToken = default) =>
        new(
            text,
            parameters,
            context.Database.CurrentTransaction?.GetDbTransaction(),
            timeout ?? context.Database.GetCommandTimeout() ?? 30,
            type,
            cancellationToken: cancellationToken);

    public static async Task<IEnumerable<T>> QueryAsync<T>(
        this DbContext context,
        string text,
        object? parameters = null,
        int? timeout = null,
        CommandType type = CommandType.Text,
        CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        var command = context.CreateCommand(text, parameters, timeout, type, cancellationToken);
        return await connection.QueryAsync<T>(command);
    }

    public static async Task<int> ExecuteAsync(
        this DbContext context,
        string text,
        object? parameters = null,
        int? timeout = null,
        CommandType type = CommandType.Text,
        CancellationToken cancellationToken = default)
    {
        var connection = context.Database.GetDbConnection();
        var command = context.CreateCommand(text, parameters, timeout, type, cancellationToken);
        return await connection.ExecuteAsync(command.CommandText);
    }

    public static async Task ExecuteInTransactionAsync(this DbContext db, Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        using TransactionScope transaction =
            new(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
        await action(cancellationToken);
        transaction.Complete();
    }

    public static IEnumerable<EntityChange> GetChanges(this DbContext dbContext) =>
        dbContext.ChangeTracker.Entries().Select(ToEntityChange).Where(x => x is not null)
            .Cast<EntityChange>();

    private static EntityChange? ToEntityChange(EntityEntry entityEntry)
    {
        var entity = entityEntry.Entity;

        ChangeAction changeAction;
        switch (entityEntry.State)
        {
            case EntityState.Added:
                changeAction = ChangeAction.Inserted;
                break;

            case EntityState.Modified:
                changeAction = ChangeAction.Updated;
                break;

            case EntityState.Deleted:
                changeAction = ChangeAction.Deleted;
                break;

            case EntityState.Detached:
            case EntityState.Unchanged:
            default:
                return null;
        }

        var entityName = entity.GetType().FullName!;
        var idProperty = entityEntry.Properties.SingleOrDefault(p => p.Metadata.IsPrimaryKey());
        if (idProperty?.CurrentValue is null)
        {
            throw new InvalidOperationException($"{entityName} has no primary key");
        }

        var entityId = idProperty.CurrentValue.ToString()!;
        var changes = entityEntry.Properties.Select(ToPropertyChange).ToArray();
        return new EntityChange(entityId, entityName, changeAction, changes);
    }

    private static PropertyChange ToPropertyChange(this PropertyEntry propertyEntry) =>
        new(propertyEntry.Metadata.Name, propertyEntry.CurrentValue?.ToString(),
            propertyEntry.OriginalValue?.ToString());
}
