namespace Sheeva.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class ConfigurationExtensions
{
    public static T GetConfiguration<T>(this IServiceProvider serviceProvider) where T : class, new()
    {
        var configOptions = serviceProvider.GetRequiredService<IConfigureOptions<T>>();
        var config = new T();
        configOptions.Configure(config);
        return config;
    }
}
