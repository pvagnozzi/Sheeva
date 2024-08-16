namespace Sheeva.Data.Abstractions.Specifications;

public interface IIncludeExpression : IPropertyExpression
{
    IIncludeExpression[] NestedExpressions { get; }
}
