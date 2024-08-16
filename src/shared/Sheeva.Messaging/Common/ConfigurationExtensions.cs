namespace Sheeva.Messaging.Common;

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sheeva.Data.Abstractions;
using Mapping;
using Abstractions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services) =>
        services.AddMessaging(Assembly.GetEntryAssembly()!);

    public static IServiceCollection AddMessaging(this IServiceCollection services,
        IEnumerable<Assembly> assemblies) =>
        services
            .AddScoped<IRequestPublisher, RequestPublisher>()
            .AddScoped<IRequestHandlerContext>(ctx =>
                new RequestHandlerContext(
                    ctx.GetRequiredService<IMapper>(),
                    ctx.GetRequiredService<IUnitOfWork>(),
                    ctx.GetService<IRequestNotificationService>()))
            .AddMediatR(cfg =>
            {
                foreach (var asm in assemblies)
                {
                    cfg.RegisterServicesFromAssembly(asm);
                }
            });

    private static IServiceCollection AddMessaging(this IServiceCollection services,
        Assembly asm) => services.AddMessaging(new[] { asm });

}
