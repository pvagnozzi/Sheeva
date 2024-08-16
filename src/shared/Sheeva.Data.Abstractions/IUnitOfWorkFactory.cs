namespace Sheeva.Data.Abstractions;

public interface IUnitOfWorkFactory : IDisposable
{
    IUnitOfWork CreateUnitOfWork();
}
