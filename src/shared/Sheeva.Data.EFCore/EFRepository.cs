namespace Sheeva.Data.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Core;
using Abstractions;

// ReSharper disable once InconsistentNaming
public class EFRepository<TKey, TEntity> : EFReadOnlyRepository<TKey, TEntity>, IRepository<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    #region CTOR
    protected internal EFRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
    }
    #endregion

    #region Async Methods
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogTrace("AddAsync entity {entity}", entity);
            await this.DbSet.AddAsync(entity, cancellationToken);
            this.Logger.LogTrace("AddAsync entity {entity}: ok", entity);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "AddAsync entity {entity}: {Exception}", entity, ex);
            throw;
        }

    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.Logger.LogTrace("UpdateAsync entity {entity}", entity);
            entity = this.Attach(entity);
            this.DbSet.Update(entity);
            this.Logger.LogTrace("UpdateAsync entity {entity}: ok", entity);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "UpdateAsync entity {entity}: {Exception}", entity, ex);
            throw;
        }
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            this.Logger.LogTrace("DeleteAsync entity {entity}", entity);
            entity = this.Attach(entity);
            this.DbSet.Remove(entity);
            this.Logger.LogTrace("DeleteAsync entity {entity}: ok", entity);
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "DeleteAsync entity {entity}: {Exception}", entity, ex);
            throw;
        }
    }

    public async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace("DeleteByIdAsync id {id}", id);
        var entity = await this.GetByIdAsync(id, cancellationToken);
        if (entity is null)
        {
            throw new NotFoundDomainException($"{typeof(TKey).Name} {id} not found");
        }

        await this.DeleteAsync(entity, cancellationToken);
        this.Logger.LogTrace("DeleteByIdAsync id {id}: ok", id);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogTrace("AddRangeAsync entities {entities}", entities);
            await this.DbSet.AddRangeAsync(entities, cancellationToken);
            this.Logger.LogTrace("AddRangeAsync entities {entities}: ok", entities);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "AddRangeAsync entities {entities}: {Exception}", entities, ex);
            throw;
        }
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace("UpdateRangeAsync entities {entities}", entities);
        foreach (var entity in this.Attach(entities))
        {
            await this.UpdateAsync(entity, cancellationToken);
        }
        this.Logger.LogTrace("UpdateRangeAsync entities {entities}: ok", entities);
    }

    public async Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace("DeleteRangeAsync entities {entities}", entities);
        foreach (var entity in this.Attach(entities))
        {
            await this.DeleteAsync(entity, cancellationToken);
        }
        this.Logger.LogTrace("DeleteRangeAsync entities {entities}: ok", entities);
    }

    public async Task DeleteRangeByIdAsync(IEnumerable<TKey> ids, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace("DeleteRangeByIdAsync ids {ids}", ids);
        foreach (var id in ids)
        {
            await this.DeleteByIdAsync(id, cancellationToken);
        }
        this.Logger.LogTrace("DeleteRangeByIdAsync ids {ids}: ok", ids);
    }
    #endregion

    #region Sync Methods
    public void Add(TEntity entity)
    {
        try
        {
            this.Logger.LogTrace("Add entity {entity}", entity);
            this.DbSet.Add(entity);
            this.Logger.LogTrace("Add entity {entity}: ok", entity);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Add entity {entity}: {Exception}", entity, ex);
            throw;
        }
    }

    public void Update(TEntity entity)
    {
        try
        {
            this.Logger.LogTrace("Update entity {entity}", entity);
            entity = this.Attach(entity);
            this.DbSet.Update(entity);
            this.Logger.LogTrace("Update entity {entity}: ok", entity);
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "Update entity {entity}: {Exception}", entity, e);
            throw;
        }

    }

    public void Delete(TEntity entity)
    {
        try
        {
            this.Logger.LogTrace("Delete entity {entity}", entity);
            entity = this.Attach(entity);
            this.DbSet.Remove(entity);
            this.Logger.LogTrace("Delete entity {entity}: ok", entity);
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "Delete entity {entity}: {Exception}", entity, e);
            throw;
        }
    }

    public void DeleteById(TKey id)
    {
        this.Logger.LogTrace("DeleteById id {id}", id);
        var entity = this.GetById(id);
        if (entity is null)
        {
            throw new NotFoundDomainException($"{typeof(TKey).Name} {id} not found");
        }

        this.Delete(entity);
        this.Logger.LogTrace("DeleteById id {id}: ok", id);
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        try
        {
            this.Logger.LogTrace("AddRange entities {entities}", entities);
            this.DbSet.AddRange(entities);
            this.Logger.LogTrace("AddRange entities {entities}: ok", entities);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "AddRange entities {entities}: {Exception}", entities, ex);
            throw;
        }
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        this.Logger.LogTrace("UpdateRange entities {entities}", entities);
        foreach (var entity in this.Attach(entities))
        {
            this.UpdateAsync(entity);
        }
        this.Logger.LogTrace("UpdateRange entities {entities}: ok", entities);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        this.Logger.LogTrace("DeleteRange entities {entities}", entities);
        foreach (var entity in this.Attach(entities))
        {
            this.Delete(entity);
        }
        this.Logger.LogTrace("DeleteRange entities {entities}: ok", entities);
    }

    public void DeleteRangeById(IEnumerable<TKey> ids)
    {
        this.Logger.LogTrace("DeleteRangeById ids {ids}", ids);
        foreach (var id in ids)
        {
            this.DeleteById(id);
        }
        this.Logger.LogTrace("DeleteRangeById ids {ids}: ok", ids);
    }
    #endregion

    #region Internal Methods
    // ReSharper disable once MemberCanBePrivate.Global
    protected IEnumerable<TEntity> Attach(IEnumerable<TEntity> entities) => entities.Select(this.Attach);

    // ReSharper disable once MemberCanBePrivate.Global
    protected TEntity Attach(TEntity entity)
    {
        this.Logger.LogTrace("Attach entity {entity}", entity);
        var entry = this.DbSet.Entry(entity);
        if (entry.State != EntityState.Detached)
        {
            return entity;
        }

        this.Logger.LogTrace("Attach entity {entity}: detach", entity);
        this.DbSet.Attach(entity);
        this.Logger.LogTrace("Attach entity {entity}: attached", entity);
        return entity;
    }
    #endregion
}
