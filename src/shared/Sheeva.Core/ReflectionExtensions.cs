namespace Sheeva.Core;

using System.Reflection;

public static class ReflectionExtensions
{
    public static bool Implements<T>(this Type type) => type.IsAssignableTo(typeof(T));

    public static bool Implements(this Type type, Type implementingType) => type.IsAssignableTo(implementingType);

    public static Type[] GetTypesImplementing<T>(this Assembly assembly) =>
        assembly.GetTypes().Where(t => t.Implements<T>()).ToArray();

    public static Type[] GetTypesImplementing(this Assembly assembly, Type type) =>
        assembly.GetTypes().Where(t => t.Implements(type)).ToArray();
}
