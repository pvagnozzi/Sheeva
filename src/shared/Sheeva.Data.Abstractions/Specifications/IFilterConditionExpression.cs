namespace Sheeva.Data.Abstractions.Specifications;

public interface IFilterConditionExpression : IPropertyExpression
{
    FilterConditionOperator ConditionOperator { get; }

    object? Value { get; }
}
