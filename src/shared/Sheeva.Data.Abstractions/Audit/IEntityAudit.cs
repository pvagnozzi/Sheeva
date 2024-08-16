namespace Sheeva.Data.Abstractions.Audit;

using Abstractions;

public interface IEntityAudit : IEntity<string>
{
    string EntityId { get; }

    string EntityName { get; }

    ChangeAction Action { get; }

    IPropertyAudit[] Properties { get; }
}
