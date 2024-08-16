namespace Sheeva.Mapping;

using System.Reflection;
using global::MapsterMapper;
using Mapster;
using InnerMapper = global::MapsterMapper.IMapper;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddMapsterMapping(this IServiceCollection services,
        Assembly[]? assemblies = null, Action<TypeAdapterConfig>? options = null) =>
        services
            .AddMapster(assemblies, options)
            .AddScoped<IMapper, MapsterMapper>(sp => new MapsterMapper(sp.GetRequiredService<InnerMapper>()));

    private static IServiceCollection AddMapster(this IServiceCollection services, Assembly[]? assemblies = null,
        Action<TypeAdapterConfig>? options = null)
    {

        if (assemblies is null || assemblies.Length == 0)
        {
            assemblies = [Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()];
        }

        var config = new TypeAdapterConfig();
        config.Scan(assemblies);
        options?.Invoke(config);
        return services
            .AddSingleton(config)
            .AddScoped<InnerMapper, ServiceMapper>();
    }
}
