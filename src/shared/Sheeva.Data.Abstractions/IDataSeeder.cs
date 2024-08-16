namespace Sheeva.Data.Abstractions;

using Services;

public interface IDataSeeder : IService
{
    void SeedData();
}
