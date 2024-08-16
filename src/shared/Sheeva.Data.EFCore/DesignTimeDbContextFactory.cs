namespace Sheeva.Data.EFCore;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public abstract class DesignTimeDbContextFactory<T>(string connectionStringName, string defaultConnectionString)
    : IDesignTimeDbContextFactory<T>
    where T : DbContext
{
    public T CreateDbContext(string[] args)
    {
        IConfiguration config = BuildConfiguration(new ConfigurationBuilder()).Build();
        var connectionsString = args.Length > 0
            ? this.GetConnectionStringFromConfiguration(config, this.GetConnectionStringName())
            : string.Join(';', args);

        if (string.IsNullOrEmpty(connectionsString))
        {
            connectionsString = defaultConnectionString;
        }

        var dbContextOptionsBuilder =
            this.BuildDbContextOptionsBuilder(new DbContextOptionsBuilder<T>(), connectionsString);
        return this.CreateDbContext(dbContextOptionsBuilder.Options);
    }

    private string GetConnectionStringName() => $"ConnectionStrings:{connectionStringName}";

    private string? GetConnectionStringFromConfiguration(IConfiguration config, string name) => config.GetSection(name).Value;

    private static IConfigurationBuilder BuildConfiguration(IConfigurationBuilder builder) =>
        builder
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .AddEnvironmentVariables();

    protected virtual DbContextOptionsBuilder<T> BuildDbContextOptionsBuilder(DbContextOptionsBuilder<T> builder,
        string connectionString) =>
        builder.UseNpgsql(connectionString);

    protected abstract T CreateDbContext(DbContextOptions<T> options);
}
