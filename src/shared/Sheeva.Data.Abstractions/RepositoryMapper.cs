namespace Sheeva.Data.Abstractions;

using System.Collections.Concurrent;
using System.Reflection;

public abstract class RepositoryMapper : IRepositoryMapper
{
    private readonly IDictionary<Type, Type> repositoryTypes;

    protected RepositoryMapper(IEnumerable<Assembly>? assemblies = null)
    {
        this.repositoryTypes = new ConcurrentDictionary<Type, Type>();
        this.RegisterFromAssemblies(assemblies?.ToArray() ?? []);
    }

    public IRepositoryMapper Register(Type repositoryType)
    {
        var entityType = this.GetEntityType(repositoryType);
        if (entityType is null)
        {
            throw new InvalidCastException($"{repositoryType.FullName} is not a valid Repository type");
        }

        this.repositoryTypes.TryAdd(entityType, repositoryType);
        return this;
    }

    public IRepositoryMapper RegisterFromAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var entityType = this.GetEntityType(type);
            if (entityType is null)
            {
                continue;
            }

            this.Register(entityType, type);
        }

        return this;
    }

    public IRepositoryMapper RegisterFromAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            this.RegisterFromAssembly(assembly);
        }

        return this;
    }

    public Type? GetRepositoryType(Type entityType) => this.repositoryTypes.TryGetValue(entityType, out Type? repositoryType) ? repositoryType : null;

    protected abstract bool IsValidRepositoryType(Type repository, Type keyType, Type entityType);

    private void Register(Type entityType, Type repositoryType) => this.repositoryTypes.TryAdd(entityType, repositoryType);

    private Type? GetEntityType(Type repositoryType)
    {
        if (!repositoryType.IsClass || repositoryType.IsAbstract)
        {
            return null;
        }

        var interfaceType = (from interFace in repositoryType.GetInterfaces()
            where interFace.IsGenericType && interFace.GetGenericTypeDefinition() == typeof(IRepository<,>)
            select interFace).FirstOrDefault();
        if (interfaceType is null)
        {
            return null;
        }

        var arguments = interfaceType.GetGenericArguments();
        var keyType = arguments[0];
        var entityType = arguments[1];

        return this.IsValidRepositoryType(repositoryType, keyType, entityType) ? entityType : null;
    }
}
