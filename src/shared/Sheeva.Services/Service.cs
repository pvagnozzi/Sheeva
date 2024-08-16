namespace Sheeva.Services;

using Microsoft.Extensions.Logging;
using Core;

public class Service : Disposable, IService
{
    protected Service(ILogger logger) => this.Logger = logger;

    protected ILogger Logger { get; }
}
