namespace Sheeva.Data.Abstractions.Specifications;

using System.Linq.Expressions;
using Abstractions;

public record PagedSpecification<TKey, TEntity> : Specification<TKey, TEntity>, IPagedSpecification<TKey, TEntity>
    where TKey : IEquatable<TKey>
    where TEntity : IEntity<TKey>
{
    public PagedSpecification(
        Expression<Func<TEntity, bool>>? where = null,
        IEnumerable<IFilterConditionExpression>? filterExpressions = null,
        IEnumerable<IIncludeExpression>? includeExpressions = null,
        IEnumerable<ISortExpression>? sortExpressions = null,
        int pageIndex = 0,
        int pageSize = 10) : base(where, filterExpressions, includeExpressions, sortExpressions)
    {
        this.PageIndex = pageIndex;
        this.PageSize = pageSize;
    }

    public int PageIndex { get; }

    public int PageSize { get; }
}
