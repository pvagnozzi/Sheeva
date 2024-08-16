namespace Sheeva.Data.Abstractions;

using Microsoft.Extensions.Logging;
using Services;

public abstract class DataSeeder(IUnitOfWork unitOfWork, ILogger logger) : Service(logger), IDataSeeder
{
    private IUnitOfWork UnitOfWork { get; } = unitOfWork;

    public void SeedData()
    {
        try
        {
            this.Logger.LogInformation("SeedData");
            this.SeedEntities();
            this.Logger.LogInformation("SeedData: completed");
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "SeedData: {Exception}", ex);
        }
    }

    protected abstract void SeedEntities();

    protected void SeedEntity<TKey, TEntity>(IEnumerable<TEntity> entities)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        try
        {
            this.Logger.LogTrace("SeedEntity {TEntity}", typeof(TEntity).Name);
            using var repository = this.UnitOfWork.GetRepository<TKey, TEntity>();
            foreach (var entity in entities)
            {
                try
                {
                    this.Logger.LogTrace("SeedEntity entity {entity}", entity);
                    repository.Add(entity);
                    this.Logger.LogTrace("SeedEntity entity {entity}: inserted", entity);

                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, "SeedEntity entity {entity}: {Exception}", entity, ex);
                }
            }

            this.UnitOfWork.SaveChanges();
            this.Logger.LogTrace("SeedEntity {TEntity}: completed", typeof(TEntity).Name);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "SeedEntity {TEntity}: {Exception}", ex);
        }
    }

    protected void SeedEntity<TKey, TEntity>(IEnumerable<TEntity> entities, Func<TEntity, TEntity, bool> finder)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        try
        {
            this.Logger.LogTrace("SeedEntity {TEntity}", typeof(TEntity).Name);
            using var repository = this.UnitOfWork.GetRepository<TKey, TEntity>();
            var currentItems = repository.List();

            foreach (var entity in entities)
            {
                try
                {
                    this.Logger.LogTrace("SeedEntity entity {entity}", entity);
                    var currentItem = currentItems.FirstOrDefault(x => finder(x, entity));
                    if (currentItem is not null)
                    {
                        this.Logger.LogTrace("SeedEntity entity {entity}: found", entity);
                        continue;
                    }

                    repository.Add(entity);
                    this.Logger.LogTrace("SeedEntity entity {entity}: inserted", entity);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex, "SeedEntity entity {entity}: {Exception}", entity, ex);
                }
            }

            this.UnitOfWork.SaveChanges();
            this.Logger.LogTrace("SeedEntity {TEntity}: completed", typeof(TEntity).Name);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "SeedEntity {TEntity}: {Exception}", ex);
        }
    }
}
