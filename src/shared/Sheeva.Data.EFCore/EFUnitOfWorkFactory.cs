namespace Sheeva.Data.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Abstractions;
using Audit;

// ReSharper disable once InconsistentNaming
public class EFUnitOfWorkFactory<T>(IServiceProvider serviceProvider) : UnitOfWorkFactory(serviceProvider)
    where T : DbContext
{
    private T CreateDbContext()
    {
        try
        {
            this.Logger.LogDebug("CreateDbContext");
            var result = this.ServiceProvider.GetRequiredService<T>();
            this.Logger.LogDebug("CreateDbContext: completed");
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "CreateDbContext: {Ex}", ex.Message);
            throw;
        }
    }

    protected override IUnitOfWork CreateUnitOfWorkInstance(IRepositoryMapper repositoryMapper, ILoggerFactory loggerFactory)
    {
        try
        {
            this.Logger.LogDebug("CreateUnitOfWork");
            var changesSerializer = this.ServiceProvider.GetService<IEntityChangesSerializer>();
            EFUnitOfWork result = new(repositoryMapper, this.CreateDbContext(), changesSerializer, loggerFactory);
            this.Logger.LogDebug("CreateUnitOfWork: completed");
            return result;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "CreateUnitOfWork: {Ex}", ex.Message);
            throw;
        }
    }
}
