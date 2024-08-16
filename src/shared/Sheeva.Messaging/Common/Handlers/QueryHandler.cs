namespace Sheeva.Messaging.Common.Handlers;

using MediatR;
using Microsoft.Extensions.Logging;
using Core;
using Abstractions;
using Queries;

public abstract class QueryHandler<TQuery, TResponse>(IRequestHandlerContext context, ILogger logger) : RequestHandler(context, logger),
    IRequestHandler<TQuery, TResponse>
    where TQuery : Query<TResponse>, IRequest<TResponse>
{
    public virtual async Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var typeName = this.GetType().FullName;

        try
        {
            this.Logger.LogTrace("Query handler {TypeName} / {Request}", typeName, request);
            var result = await this.HandleQueryAsync(request, cancellationToken);
            this.Logger.LogTrace("Query handler {TypeName} / {Request} completed successfully: {Result}", typeName,
                request, result);
            return result;
        }
        catch (DomainException ex)
        {
            this.Logger.LogError(ex, "Query handler {TypeName} / {Request}: error {Message}", typeName, request, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "Query handler {TypeName} / {Request}: error {Message}", typeName, request, ex.Message);
            throw new DomainException(ex.Message, request.CorrelationId, ex);
        }
    }

    protected abstract Task<TResponse> HandleQueryAsync(TQuery query, CancellationToken cancellationToken);
}
