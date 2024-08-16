namespace Sheeva.Data.Abstractions.Specifications;

using System.Linq.Expressions;
using Abstractions;

public interface ISpecification<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : IEntity<TKey>
{
    Expression<Func<TEntity, bool>>? Where { get; }

    IIncludeExpression[] Includes { get; }

    IFilterConditionExpression[] Filters { get; }

    ISortExpression[] Sort { get; }
}
