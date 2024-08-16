namespace Sheeva.Data.EFCore;

using Microsoft.Extensions.Logging;
using Abstractions;
using Services;

public class UnitOfWorkService : Service
{
    protected UnitOfWorkService(IUnitOfWork unitOfWork, ILogger logger) : base(logger) => this.UnitOfWork = unitOfWork;

    protected IUnitOfWork UnitOfWork { get; }

    protected Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        this.UnitOfWork.SaveChangesAsync(cancellationToken);

    protected override void DisposeResources()
    {
        this.UnitOfWork.Dispose();
        base.DisposeResources();
    }
}
