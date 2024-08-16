namespace Sheeva.Data.Abstractions.Specifications;

using System.Linq.Expressions;
using Abstractions;

public record Specification<TKey, TEntity> : ISpecification<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : IEntity<TKey>
{
    public Specification(
        Expression<Func<TEntity, bool>>? where = null,
        IEnumerable<IFilterConditionExpression>? filterExpressions = null,
        IEnumerable<IIncludeExpression>? includeExpressions = null,
        IEnumerable<ISortExpression>? sortExpressions = null)
    {
        this.Where = where;
        this.Filters = filterExpressions?.ToArray() ?? [];
        this.Includes = includeExpressions?.ToArray() ?? [];
        this.Sort = sortExpressions?.ToArray() ?? [];
    }

    public Expression<Func<TEntity, bool>>? Where { get; }

    public IFilterConditionExpression[] Filters { get; }

    public IIncludeExpression[] Includes { get; }

    public ISortExpression[] Sort { get; }
}
