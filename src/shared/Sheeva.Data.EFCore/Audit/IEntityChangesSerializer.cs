namespace Sheeva.Data.EFCore.Audit;

using EFCore;

public interface IEntityChangesSerializer
{
    Task HandleChangesAsync(EFUnitOfWork unitOfWork, string userId, IEnumerable<Abstractions.Audit.EntityChange> changes,
        CancellationToken cancellationToken = default);
}
