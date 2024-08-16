namespace Sheeva.Data.EFCore;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Abstractions;

public static class ConfigurationExtensions
{
    // ReSharper disable once InconsistentNaming
    public static IServiceCollection AddEFUnitOfWorkFactory<T>(
        this IServiceCollection serviceCollection)
        where T : DbContext
    {
        serviceCollection.AddSingleton<IRepositoryMapper, EFRepositoryMapper>(_ =>
            new EFRepositoryMapper(typeof(T).Assembly));
        serviceCollection.AddScoped<IUnitOfWorkFactory, EFUnitOfWorkFactory<T>>(ctx => new EFUnitOfWorkFactory<T>(ctx));
        serviceCollection.AddScoped<IUnitOfWork>(ctx =>
            ctx.GetRequiredService<IUnitOfWorkFactory>().CreateUnitOfWork());
        return serviceCollection;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static DbContextOptionsBuilder ConfigurePostgres(
        this DbContextOptionsBuilder builder,
        string connectionString,
        int maxRetryCount = 15,
        int retryDelaySeconds = 30,
        string? migrationsAssemblyName = null)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(connectionString));
        }

        return builder.UseNpgsql(connectionString, optionsBuilder =>
        {
            if (!string.IsNullOrEmpty(migrationsAssemblyName))
            {
                optionsBuilder.MigrationsAssembly(migrationsAssemblyName);
            }

            optionsBuilder.EnableRetryOnFailure(maxRetryCount, TimeSpan.FromSeconds(retryDelaySeconds), null);
        });
    }

    private static DbContextOptionsBuilder ConfigurePostgres(
        this DbContextOptionsBuilder builder,
        string connectionString,
        Assembly migrationsAssembly,
        int maxRetryCount = 15,
        int retryDelaySeconds = 30) =>
        builder.ConfigurePostgres(connectionString, maxRetryCount, retryDelaySeconds,
            migrationsAssembly.GetName().Name);

    // ReSharper disable once MemberCanBePrivate.Global
    public static DbContextOptionsBuilder ConfigurePostgres(
        this DbContextOptionsBuilder builder,
        string connectionString,
        Type dbContextType,
        int maxRetryCount = 15,
        int retryDelaySeconds = 30) =>
        builder.ConfigurePostgres(connectionString, dbContextType.Assembly, maxRetryCount, retryDelaySeconds);

    public static DbContextOptionsBuilder ConfigurePostgres<T>(
        this DbContextOptionsBuilder builder,
        string connectionString,
        int maxRetryCount = 15,
        int retryDelaySeconds = 30)
        where T : DbContext =>
        builder.ConfigurePostgres(connectionString, typeof(T), maxRetryCount, retryDelaySeconds);

    // ReSharper disable once MemberCanBePrivate.Global
    public static void ApplyMigrations<T>(this T dbContext, ILogger logger)
        where T : DbContext
    {
        var dbContextName = dbContext.GetType().FullName;

        try
        {
            var database = dbContext.Database;
            logger.LogTrace("Migration '{DbContext}': pending migrations", dbContextName);
            var pendingMigrations = database.GetPendingMigrations();

            if (pendingMigrations.Any())
            {
                logger.LogTrace("Migration '{DbContext}': apply {PendingMigrations}", dbContextName,
                    pendingMigrations);
                dbContext.Database.Migrate();
                logger.LogTrace("Migration '{DbContext}': completed", dbContextName);
            }
            else
            {
                logger.LogTrace("Migration '{DbContext}': no migrations", dbContextName);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Migration '{DbContext}': {Ex}", dbContextName, ex.Message);
            throw;
        }
    }

}
