namespace Sheeva.Messaging.Common.Handlers;

using Microsoft.Extensions.Logging;
using Core;
using Abstractions;

public abstract class RequestHandler(IRequestHandlerContext context, ILogger logger) : Disposable
{
    protected IRequestHandlerContext Context { get; } = context;

    protected ILogger Logger { get; } = logger;
}
