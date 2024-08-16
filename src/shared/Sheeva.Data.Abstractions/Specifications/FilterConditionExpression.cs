namespace Sheeva.Data.Abstractions.Specifications;

public record FilterConditionExpression(string PropertyName, FilterConditionOperator ConditionOperator, object? Value = null)
    : PropertyExpression(PropertyName), IFilterConditionExpression;
