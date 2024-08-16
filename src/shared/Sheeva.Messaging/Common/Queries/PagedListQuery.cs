namespace Sheeva.Messaging.Common.Queries;

using Core;
using Abstractions;

public abstract record PagedListQuery<TModel> : Query<IPagedList<TModel>>
{
    protected PagedListQuery(IApplicationContext? context = null,
        string? correlationId = null) : base(context,
        correlationId)
    {
    }

    public int PageIndex { get; init; } = 0;

    public int PageSize { get; init; } = 10;
}
