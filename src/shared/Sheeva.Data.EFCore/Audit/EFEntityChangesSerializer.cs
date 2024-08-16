namespace Sheeva.Data.EFCore.Audit;

using EFCore;

// ReSharper disable once InconsistentNaming
public class EFEntityChangesSerializer : IEntityChangesSerializer
{
    public async Task HandleChangesAsync(EFUnitOfWork unitOfWork, string userId, IEnumerable<Abstractions.Audit.EntityChange> changes,
        CancellationToken cancellationToken = default)
    {
        var audits = changes.Select(x => x.ToEntityAudit());
        using var repository = unitOfWork.GetRepository<string, EntityAudit>();
        await repository.AddRangeAsync(audits, cancellationToken);
        await unitOfWork.SaveDbContextChangesAsync(cancellationToken);
    }

}
