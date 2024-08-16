namespace Sheeva.Data.Abstractions;

public interface IDataServiceFactory : IDisposable
{
    IDataService CreateDataService();
}
