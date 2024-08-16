namespace Sheeva.Data.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Abstractions;
using Abstractions.Audit;
using Audit;

// ReSharper disable once InconsistentNaming
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class EFUnitOfWork : UnitOfWork
{
    private DbContext DbContext { get; }

    private readonly IEntityChangesSerializer? changesSerializer;

    internal EFUnitOfWork(
        IRepositoryMapper repositoryMapper,
        DbContext dbContext,
        IEntityChangesSerializer? changesSerializer,
        ILoggerFactory loggerFactory) : base(repositoryMapper, loggerFactory)
    {
        this.DbContext = dbContext;
        this.changesSerializer = changesSerializer;
    }

    protected override IRepositoryFactory BuildRepositoryFactory(IRepositoryMapper repositoryMapper,
        ILoggerFactory loggerFactory) =>
        new EFRepositoryFactory(this.DbContext, repositoryMapper, loggerFactory);

    protected override void DisposeUnitOfWork() => this.DbContext.Dispose();

    protected virtual Task<string> GetUserIdAsync() => Task.FromResult(string.Empty);

    protected override async Task SaveUnitOfWorkAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogDebug("SaveUnitOfWorkAsync");
            if (this.DbContext.ChangeTracker.HasChanges())
            {

                this.Logger.LogTrace("SaveUnitOfWorkAsync: {Count} changes", this.DbContext.ChangeTracker.Entries().Count());
                await this.DbContext.SaveChangesAsync(cancellationToken);
                var changes = this.changesSerializer is not null ? this.DbContext.GetChanges() : null;
                await this.SaveDbContextChangesAsync(cancellationToken);
                this.Logger.LogTrace("SaveUnitOfWorkAsync: changes cleared");
                this.Logger.LogTrace("SaveUnitOfWorkAsync: changes saved");

                if (changes is not null)
                {
                    var userId = await this.GetUserIdAsync();
                    this.Logger.LogTrace("SaveUnitOfWorkAsync: saving audit");
                    await this.SerializeChangesAsync(userId, changes, cancellationToken);
                    this.Logger.LogTrace("SaveUnitOfWorkAsync: audit saved");
                }
            }
            this.Logger.LogDebug("SaveUnitOfWorkAsync: completed");
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "SaveUnitOfWork: {Ex}", ex.Message);
            throw;
        }
    }

    protected override void SaveUnitOfWork()
    {
        try
        {
            this.Logger.LogDebug("SaveUnitOfWork");
            if (this.DbContext.ChangeTracker.HasChanges())
            {

                this.Logger.LogTrace("SaveUnitOfWork: {Count} changes", this.DbContext.ChangeTracker.Entries().Count());
                this.DbContext.SaveChanges();
                var changes = this.changesSerializer is not null ? this.DbContext.GetChanges() : null;
                this.SaveDbContextChanges();
                this.Logger.LogTrace("SaveUnitOfWork: changes cleared");
                this.Logger.LogTrace("SaveUnitOfWork: changes saved");
            }
            this.Logger.LogDebug("SaveUnitOfWork: completed");
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "SaveUnitOfWork: {Ex}", ex.Message);
            throw;
        }
    }

    protected internal async Task SaveDbContextChangesAsync(CancellationToken cancellationToken = default)
    {
        await this.DbContext.SaveChangesAsync(cancellationToken);
        this.DbContext.ChangeTracker.Clear();
    }

    private void SaveDbContextChanges()
    {
        this.DbContext.SaveChanges();
        this.DbContext.ChangeTracker.Clear();
    }

    private Task SerializeChangesAsync(string userId, IEnumerable<EntityChange> changes, CancellationToken cancellationToken = default) =>
        this.changesSerializer?.HandleChangesAsync(this, userId, changes, cancellationToken) ?? Task.CompletedTask;
}
