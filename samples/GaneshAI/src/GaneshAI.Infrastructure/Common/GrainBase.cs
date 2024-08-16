namespace GaneshAI.Infrastructure.Common;

using Microsoft.Extensions.Logging;

public class GrainBase(ILogger logger) : Grain
{
    public ILogger Logger { get; } = logger;
}
