namespace Sheeva.Messaging.Common.Queries;

using Abstractions;

// ReSharper disable once ClassNeverInstantiated.Global
public record ListQuery<TModel> : Query<IList<TModel>>
{
    public ListQuery(IApplicationContext? context = null, string? correlationId = null) : base(context,
        correlationId)
    {
    }
}
