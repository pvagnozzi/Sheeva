namespace Sheeva.Data.Abstractions.Specifications;

public record SortExpression(string PropertyName, bool Descending = false) : PropertyExpression(PropertyName), ISortExpression;
