using GaneshAI.Abstractions;
using Microsoft.Extensions.Logging;

namespace GaneshAI.Infrastructure;

using Common;

public static class HelloGrainLogMessages
{
    private static readonly Action<ILogger, string, Exception?> SayHelloMessageReceived =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(HelloGrain)),
            "SayHello message received: greeting = \"{Greeting}\"");

    public static void LogSayHelloMessageReceived(this ILogger logger, string greeting) => SayHelloMessageReceived(logger, greeting, null);
}

public class HelloGrain(ILogger<HelloGrain> logger) : GrainBase(logger), IHello
{
    ValueTask<string> IHello.SayHello(string greeting)
    {
        this.Logger.LogSayHelloMessageReceived(greeting);

        return ValueTask.FromResult($"""
                                         Client said: "{greeting}", so HelloGrain says: Hello!
                                         """);
    }
}
