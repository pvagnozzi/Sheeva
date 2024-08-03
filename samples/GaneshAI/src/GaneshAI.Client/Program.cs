using GaneshAI.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client =>
    {
        client.UseLocalhostClustering();
    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime();

using var host = builder.Build();
await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();

var friend = client.GetGrain<IHello>(0);
var response = await friend.SayHello("Hi friend!");

Console.WriteLine($"""
                   {response}

                   Press any key to exit...
                   """);

Console.ReadKey();
await host.StopAsync();