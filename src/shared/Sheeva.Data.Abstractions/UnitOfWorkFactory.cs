namespace Sheeva.Data.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Core;

public abstract class UnitOfWorkFactory : Disposable, IUnitOfWorkFactory
{
    protected ILogger Logger { get; }

    protected IServiceProvider ServiceProvider { get; }

    private readonly ILoggerFactory loggerFactory;

    private readonly IRepositoryMapper repositoryMapper;

    protected UnitOfWorkFactory(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
        this.repositoryMapper = serviceProvider.GetRequiredService<IRepositoryMapper>();
        this.loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        this.Logger = this.loggerFactory.CreateLogger(this.GetType());
    }

    public IUnitOfWork CreateUnitOfWork() => this.CreateUnitOfWorkInstance(this.repositoryMapper, this.loggerFactory);

    protected abstract IUnitOfWork CreateUnitOfWorkInstance(IRepositoryMapper repositoryMapper,
        ILoggerFactory loggerFactory);
}
