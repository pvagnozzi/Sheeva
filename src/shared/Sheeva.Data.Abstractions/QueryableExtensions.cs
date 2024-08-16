namespace Sheeva.Data.Abstractions;

using System.Linq.Expressions;
using Specifications;

public static class QueryableExtensions
{
    private static readonly ConstantExpression NullExpression = Expression.Constant(null);

    // ReSharper disable once MemberCanBePrivate.Global
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        => source.OrderByUsing(propertyName, "OrderBy");

    // ReSharper disable once MemberCanBePrivate.Global
    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        => source.OrderByUsing(propertyName, "OrderByDescending");

    // ReSharper disable once MemberCanBePrivate.Global
    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
        => source.OrderByUsing(propertyName, "ThenBy");

    // ReSharper disable once MemberCanBePrivate.Global
    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
        => source.OrderByUsing(propertyName, "ThenByDescending");

    public static IOrderedQueryable<T> Sort<T>(this IQueryable<T> source, IEnumerable<ISortExpression> sortExpressions)
    {
        var sorts = sortExpressions.ToArray();
        var firstSort = sorts.First();

        var result = firstSort.Descending
            ? source.OrderByDescending(firstSort.PropertyName)
            : source.OrderBy(firstSort.PropertyName);

        return sorts.Skip(1)
            .Aggregate(result, (current, sort) => sort.Descending
                ? current.ThenByDescending(firstSort.PropertyName)
                : current.ThenBy(firstSort.PropertyName));
    }

    private static IOrderedQueryable<T> OrderByUsing<T>(this IQueryable<T> source, string propertyName, string method)
    {
        var parameter = Expression.Parameter(typeof(T), "item");
        var member = parameter.ToMemberExpression(propertyName);
        var keySelector = Expression.Lambda(member, parameter);
        var methodCall = Expression.Call(typeof(Queryable), method, [parameter.Type, member.Type],
            source.Expression, Expression.Quote(keySelector));

        return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
    }

    private static Expression<Func<T, bool>> ToExpression<T>(this IFilterConditionExpression filterCondition)
    {
        var propertyName = filterCondition.PropertyName;
        var parameter = Expression.Parameter(typeof(T), "item");
        var memberExpression = propertyName.Split('.')
            .Aggregate((Expression)parameter, Expression.PropertyOrField);

        var expression =
            memberExpression.ToExpression(Expression.Constant(filterCondition.Value), filterCondition.ConditionOperator);
        return Expression.Lambda<Func<T, bool>>(expression, parameter);
    }

    public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<IFilterConditionExpression> filters) =>
        filters.Select(filter => filter.ToExpression<T>()).Aggregate(source, (current, expression) => current.Where(expression));

    private static Expression ToMemberExpression(this Expression parameter, string propertyName) =>
        propertyName.Split('.').Aggregate(parameter, Expression.PropertyOrField);

    private static Expression ToExpression(this Expression member, Expression value,
        FilterConditionOperator filterConditionOperator) =>
        filterConditionOperator switch
        {
            FilterConditionOperator.IsNull => Expression.Equal(member, NullExpression),
            FilterConditionOperator.IsNotNull => Expression.NotEqual(member, NullExpression),
            FilterConditionOperator.Equal => Expression.Equal(member, value),
            FilterConditionOperator.NotEqual => Expression.NotEqual(member, value),
            FilterConditionOperator.Lesser => Expression.LessThan(member, value),
            FilterConditionOperator.LesserEqual => Expression.LessThanOrEqual(member, value),
            FilterConditionOperator.Greater => Expression.GreaterThan(member, value),
            FilterConditionOperator.GreaterEqual => Expression.GreaterThanOrEqual(member, value),
            FilterConditionOperator.Contains => member.ToStringCallExpression(value, "Contains"),
            FilterConditionOperator.NotContains => Expression.Not(member.ToStringCallExpression(value, "Contains")),
            FilterConditionOperator.StartsWith => member.ToStringCallExpression(value, "StartsWith"),
            FilterConditionOperator.EndsWith => member.ToStringCallExpression(value, "EndsWith"),
            FilterConditionOperator.NotStartsWith => Expression.Not(member.ToStringCallExpression(value, "StartsWith")),
            FilterConditionOperator.NotEndsWith => Expression.Not(member.ToStringCallExpression(value, "EndsWith")),
            _ => throw new InvalidDataException($"Operator {filterConditionOperator} is not supported")
        };

    private static MethodCallExpression ToCallExpression<T>(this Expression memberExpression, Expression valueExpression, string methodName)
    {
        var memberType = typeof(T);
        var method = memberType.GetMethod(methodName);
        if (method is null)
        {
            throw new InvalidDataException($"{memberType} has no method {methodName}");
        }
        return Expression.Call(memberExpression, method, valueExpression);
    }

    private static MethodCallExpression ToStringCallExpression(this Expression memberExpression, Expression valueExpression,
        string methodName) =>
        memberExpression.ToCallExpression<string>(valueExpression, methodName);
}

