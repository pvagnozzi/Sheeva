namespace Sheeva.Data.Abstractions;

using Log;
using Microsoft.Extensions.Logging;
using Services;

public abstract class DataSeeder(IUnitOfWork unitOfWork, ILogger logger) : Service(logger), IDataSeeder
{
    private IUnitOfWork UnitOfWork { get; } = unitOfWork;

    private static readonly LoggerAction<string> LogSeedData = new(new EventId(1, "SeedData"), "LogSeedData: {Message}");

    private static readonly LoggerAction<string, string> LogSeedEntity = new(new EventId(2, "SeedEntity"), "SeedEntity entity {Entity}: {Message}");

    public void SeedData()
    {
        try
        {
            this.Logger.LogTrace(LogSeedData, "started");
            this.SeedEntities();
            this.Logger.LogTrace(LogSeedData, "completed");
        }
        catch (Exception ex)
        {
            this.Logger.LogError(LogSeedData, "error", ex);
        }
    }

    protected abstract void SeedEntities();

    protected void SeedEntity<TKey, TEntity>(IEnumerable<TEntity> entities)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        try
        {
            this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, "stared");
            using var repository = this.UnitOfWork.GetRepository<TKey, TEntity>();
            foreach (var entity in entities)
            {
                try
                {
                    this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, entity.ToString() ?? string.Empty);
                    repository.Add(entity);
                    this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, entity + " - completed");

                }
                catch (Exception ex)
                {
                    this.Logger.LogError(LogSeedEntity, typeof(TEntity).Name, entity + " - " + ex.Message, ex);
                }
            }

            this.UnitOfWork.SaveChanges();
            this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, "completed");
        }
        catch (Exception ex)
        {
            this.Logger.LogError(LogSeedEntity, typeof(TEntity).Name, ex.Message, ex);
        }
    }

    protected void SeedEntity<TKey, TEntity>(IEnumerable<TEntity> entities, Func<TEntity, TEntity, bool> finder)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        try
        {
            this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, "started");
            using var repository = this.UnitOfWork.GetRepository<TKey, TEntity>();
            var currentItems = repository.List();

            foreach (var entity in entities)
            {
                try
                {
                    this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, entity.ToString() ?? string.Empty);
                    var currentItem = currentItems.FirstOrDefault(x => finder(x, entity));
                    if (currentItem is not null)
                    {
                        this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name,  "found");
                        continue;
                    }

                    repository.Add(entity);
                    this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, "inserted");
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(LogSeedEntity, typeof(TEntity).Name, ex.Message);
                }
            }

            this.UnitOfWork.SaveChanges();
            this.Logger.LogTrace(LogSeedEntity, typeof(TEntity).Name, "completed");
        }
        catch (Exception ex)
        {
            this.Logger.LogError(LogSeedEntity, typeof(TEntity).Name, ex.Message);
        }
    }
}
