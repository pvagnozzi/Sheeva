namespace Sheeva.Core;

using System.Reflection;

public static class ResourceExtensions
{
    public static string[] ListResources(this Assembly assembly, string? folderName = null)
    {
        var items = assembly.GetManifestResourceNames();
        if (!string.IsNullOrEmpty(folderName))
        {
            items = items.Where(x => x.StartsWith(folderName)).ToArray();
        }

        return items;
    }

    public static string GetResourceText(this Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream is null)
        {
            throw new FileNotFoundException($"Resource '{resourceName}' not found in assembly '{assembly}'");
        }

        using StreamReader reader = new(stream);
        return reader.ReadToEnd();
    }
}
