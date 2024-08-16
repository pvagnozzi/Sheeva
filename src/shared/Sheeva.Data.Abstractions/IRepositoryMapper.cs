namespace Sheeva.Data.Abstractions;

using System.Reflection;

public interface IRepositoryMapper
{
    IRepositoryMapper Register(Type repositoryType);

    IRepositoryMapper RegisterFromAssembly(Assembly assembly);

    IRepositoryMapper RegisterFromAssemblies(IEnumerable<Assembly> assemblies);

    Type? GetRepositoryType(Type entityType);
}
