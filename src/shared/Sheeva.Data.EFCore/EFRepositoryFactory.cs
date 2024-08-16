namespace Sheeva.Data.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Abstractions;

// ReSharper disable once InconsistentNaming
public class EFRepositoryFactory(DbContext dbContext, IRepositoryMapper repositoryMapper, ILoggerFactory loggerFactory)
    : RepositoryFactory(repositoryMapper, loggerFactory)
{
    protected override object? CreateInstance(Type type, ILogger logger) =>
        Activator.CreateInstance(type, dbContext, logger);

    protected override IReadOnlyRepository<TKey, TEntity>
        CreateReadOnlyRepositoryInstance<TKey, TEntity>(ILogger logger) =>
        new EFReadOnlyRepository<TKey, TEntity>(dbContext, logger);

    protected override IRepository<TKey, TEntity> CreateRepositoryInstance<TKey, TEntity>(ILogger logger) =>
        new EFRepository<TKey, TEntity>(dbContext, logger);
}
