namespace Sheeva.Data.Abstractions.Specifications;

public interface ISortExpression : IPropertyExpression
{
    bool Descending { get; }
}
