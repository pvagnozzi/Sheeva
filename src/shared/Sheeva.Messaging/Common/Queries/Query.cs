namespace Sheeva.Messaging.Common.Queries;

using MediatR;
using Abstractions;
using Sheeva.Messaging.Abstractions.Queries;
using Common;

public abstract record Query<TModel> : RequestMessage,  IQuery<TModel>, IRequest<TModel>
{
    protected Query(IApplicationContext? context = null, string? correlationId = null) : base(context, correlationId)
    {
    }
}
