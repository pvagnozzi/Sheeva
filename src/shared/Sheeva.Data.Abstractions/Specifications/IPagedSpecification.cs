namespace Sheeva.Data.Abstractions.Specifications;

using Abstractions;

public interface IPagedSpecification<TKey, TEntity> : ISpecification<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : IEntity<TKey>
{
    int PageIndex { get; }

    int PageSize { get; }
}
