namespace Sheeva.Data.EFCore;

using Microsoft.EntityFrameworkCore;
using Core;
using Abstractions;
using Abstractions.Specifications;

public static class QueryableExtensions
{
    public static PagedList<TEntity> ToPagedList<TKey, TEntity>(this IQueryable<TEntity> source,
        int pageSize = 10,
        int pageNumber = 0)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        var count = source.Count();
        var items = source.Skip(pageNumber * pageSize).Take(pageSize).ToList();
        return new PagedList<TEntity>(items, count, pageNumber, pageSize);
    }

    public static async Task<PagedList<TEntity>> ToPagedListAsync<TKey, TEntity>(
        this IQueryable<TEntity> source,
        int pageSize = 10,
        int pageNumber = 0,
        CancellationToken cancellationToken = default)
        where TKey : IEquatable<TKey>
        where TEntity : class, IEntity<TKey>
    {
        int count = await source.CountAsync(cancellationToken);
        var items = await source.Skip(pageNumber * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PagedList<TEntity>(items, count, pageNumber, pageSize);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public static IQueryable<T> Include<T>(this IQueryable<T> source, IIncludeExpression includeExpression)
        where T: class
    {
        source.Include(includeExpression.PropertyName);

        foreach (var childExpression in includeExpression.NestedExpressions)
        {
            source.Include(childExpression);
        }

        return source;
    }

    public static IQueryable<T> Include<T>(this IQueryable<T> source,
        IEnumerable<IIncludeExpression> includeExpressions)
        where T: class
    {
        foreach (var includeExpression in includeExpressions)
        {
            source.Include(includeExpression);
        }

        return source;
    }
}
