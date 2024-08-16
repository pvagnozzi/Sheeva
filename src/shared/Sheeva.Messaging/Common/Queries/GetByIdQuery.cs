namespace Sheeva.Messaging.Common.Queries;

using Abstractions;

public abstract record GetByIdQuery<TKey, TEntity> : Query<TEntity?>
{
    protected GetByIdQuery(TKey id, IApplicationContext? context = null, string? correlationId = null) : base(context,
        correlationId) =>
        this.Id = id;

    public TKey Id { get; init; }
}
