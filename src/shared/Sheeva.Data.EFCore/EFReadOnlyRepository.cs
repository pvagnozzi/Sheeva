namespace Sheeva.Data.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Core;
using Abstractions;
using Abstractions.Specifications;

// ReSharper disable once InconsistentNaming
public class EFReadOnlyRepository<TKey, TEntity> : Disposable, IReadOnlyRepository<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : class, IEntity<TKey>
{
    #region CTOR
    protected internal EFReadOnlyRepository(DbContext dbContext, ILogger logger)
    {
        this.Logger = logger;
        this.DbContext = dbContext;
        this.DbSet = this.DbContext.Set<TEntity>();
    }
    #endregion

    #region Fields
    // ReSharper disable once MemberCanBePrivate.Global
    protected internal DbContext DbContext { get; }

    protected DbSet<TEntity> DbSet { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    protected ILogger Logger { get; }

    public IQueryable<TEntity> AsQuery() => this.GetQueryable().AsNoTracking();
    #endregion

    #region Async Methods
    public async Task<TEntity?> GetByIdAsync(TKey key, CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogDebug("GetByIdAsync {Entity}-{Key}", typeof(TEntity), key);
            var result = await this.GetDetailQueryable().FirstOrDefaultAsync(x => x.Id.Equals(key), cancellationToken);
            this.Logger.LogDebug("GetByIdAsync {Entity}-{Key}: {Result}", typeof(TEntity), key, result);
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "GetByIdAsync {Entity}-{Key}: {Ex}", typeof(TEntity), key, ex.Message);
            throw;
        }
    }

    public async Task<TEntity?> FirstOrDefaultAsync(ISpecification<TKey, TEntity> specification, CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogDebug("FirstOrDefaultAsync {Entity}-{Expression}", typeof(TEntity), specification);
            var result = await this.GetDetailQueryable().FirstOrDefaultAsync(specification.Where!, cancellationToken);
            this.Logger.LogDebug("FirstOrDefaultAsync {Entity}-{Expression}: {Result}", typeof(TEntity), specification, result);
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError("FirstOrDefaultAsync {Entity}-{Expression}: {Ex}", typeof(TEntity), specification,
                ex.Message);
            throw;
        }
    }

    public async Task<TEntity?> SingleOrDefaultAsync(ISpecification<TKey, TEntity> specification, CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogDebug("SingleOrDefaultAsync {Entity}-{Expression}", typeof(TEntity), specification);
            var result = await this.GetDetailQueryable().SingleOrDefaultAsync(specification.Where!, cancellationToken);
            this.Logger.LogDebug("SingleOrDefaultAsync {Entity}-{Expression}: {Result}", typeof(TEntity), specification, result);
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "SingleOrDefaultAsync {Entity}-{Expression}: {Ex}", typeof(TEntity), specification, ex.Message);
            throw;
        }
    }

    public async Task<IList<TEntity>> ListAsync(ISpecification<TKey, TEntity>? specification = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogDebug("ListAsync {Entity}-{Specification}", typeof(TEntity), specification);
            var result = this.GetQueryable();
            result = specification is not null ? ApplySpecification(result, specification) : result;
            var resultList = await result.ToListAsync(cancellationToken);
            this.Logger.LogDebug("ListAsync {Entity}-{Specification}: {Result}", typeof(TEntity), specification,
                resultList);
            return resultList;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "ListAsync {Entity}-{Specification}: {Ex}", typeof(TEntity), specification, ex);
            throw;
        }
    }

    public async Task<IPagedList<TEntity>> ListAsync(IPagedSpecification<TKey, TEntity> specification,
        CancellationToken cancellationToken = default)
    {
        try
        {
            this.Logger.LogDebug("ListAsync {Entity}-{Specification}", typeof(TEntity), specification);
            var result = this.GetQueryable();
            if (specification.Where is not null)
            {
                result = result.Where(specification.Where);
            }

            var listResult =
                await result.ToPagedListAsync<TKey, TEntity>(specification.PageSize, specification.PageIndex,
                    cancellationToken);
            this.Logger.LogDebug("ListAsync {Entity}-{Specification}: {Result}", typeof(TEntity), specification,
                listResult);
            return listResult;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "ListAsync {Entity}-{Specification}: {Ex}", typeof(TEntity), specification, ex);
            throw;
        }
    }
    #endregion

    #region Sync Methods
    public TEntity? GetById(TKey key)
    {
        try
        {
            this.Logger.LogDebug("GetById {Entity}-{Key}", typeof(TEntity), key);
            var result = this.GetDetailQueryable().FirstOrDefault(x => x.Id.Equals(key));
            this.Logger.LogDebug("GetById {Entity}-{Key}: {Result}", typeof(TEntity), key, result);
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "GetById {Entity}-{Key}: {Ex}", typeof(TEntity), key, ex.Message);
            throw;
        }
    }

    public TEntity? FirstOrDefault(ISpecification<TKey, TEntity> specification)
    {
        try
        {
            this.Logger.LogDebug("FirstOrDefault {Entity}-{Expression}", typeof(TEntity), specification);
            var result = this.GetDetailQueryable().FirstOrDefault(specification.Where!);
            this.Logger.LogDebug("FirstOrDefault {Entity}-{Expression}: {Result}", typeof(TEntity), specification, result);
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError("FirstOrDefault {Entity}-{Expression}: {Ex}", typeof(TEntity), specification,
                ex.Message);
            throw;
        }
    }

    public TEntity? SingleOrDefault(ISpecification<TKey, TEntity> specification)
    {
        try
        {
            this.Logger.LogDebug("SingleOrDefault {Entity}-{Expression}", typeof(TEntity), specification);
            var result = this.GetDetailQueryable().SingleOrDefault(specification.Where!);
            this.Logger.LogDebug("SingleOrDefault {Entity}-{Expression}: {Result}", typeof(TEntity), specification, result);
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "SingleOrDefault {Entity}-{Expression}: {Ex}", typeof(TEntity), specification, ex.Message);
            throw;
        }
    }

    public IList<TEntity> List(ISpecification<TKey, TEntity>? specification = null)
    {
        try
        {
            this.Logger.LogDebug("List {Entity}-{Specification}", typeof(TEntity), specification);
            var result = this.GetQueryable();
            result = specification is not null ? ApplySpecification(result, specification) : result;
            var resultList = result.ToList();
            this.Logger.LogDebug("List {Entity}-{Specification}: {Result}", typeof(TEntity), specification,
                resultList);
            return resultList;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "List {Entity}-{Specification}: {Ex}", typeof(TEntity), specification, ex);
            throw;
        }
    }

    public IPagedList<TEntity> List(IPagedSpecification<TKey, TEntity> specification)
    {
        try
        {
            this.Logger.LogDebug("List {Entity}-{Specification}", typeof(TEntity), specification);
            var result = this.GetQueryable();
            if (specification.Where is not null)
            {
                result = result.Where(specification.Where);
            }

            PagedList<TEntity> listResult =
                result.ToPagedList<TKey, TEntity>(specification.PageSize, specification.PageIndex);
            this.Logger.LogDebug("List {Entity}-{Specification}: {Result}", typeof(TEntity), specification,
                listResult);
            return listResult;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "List {Entity}-{Specification}: {Ex}", typeof(TEntity), specification, ex);
            throw;
        }
    }
    #endregion

    #region Internal Methods
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual IQueryable<TEntity> GetQueryable() => this.DbSet
        .AsQueryable()
        .AsNoTracking();

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual IQueryable<TEntity> GetDetailQueryable() => this.GetQueryable();

    private static IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> source,
        ISpecification<TKey, TEntity> specification)
    {
        source.Include(specification.Includes);
        if (specification.Where is not null)
        {
            source = source.Where(specification.Where);
        }

        return specification.Filters.Length != 0 ? source.Where(specification.Filters).Sort(specification.Sort) : source;
    }
    #endregion
}
