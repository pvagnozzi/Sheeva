namespace Sheeva.Data.EFCore;

using System.Reflection;
using Abstractions;

// ReSharper disable once InconsistentNaming
public class EFRepositoryMapper(Assembly asm) : RepositoryMapper(new[] { asm })
{
    protected override bool IsValidRepositoryType(Type repository, Type keyType, Type entityType) => !repository.IsGenericType;
}
