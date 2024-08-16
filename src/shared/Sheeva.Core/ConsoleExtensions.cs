namespace Sheeva.Core;

using System.Diagnostics;
using System.Text.Json;

public static class ConsoleExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public static string PrettyPrintJson(this string raw)
    {
        var doc = JsonDocument.Parse(raw).RootElement;
        return JsonSerializer.Serialize(doc, JsonOptions);
    }

    /// <summary>
    /// Writes green text to the console.
    /// </summary>
    /// <param name="text">The text.</param>
    [DebuggerStepThrough]
    public static void GreenWriteLine(this string text) => text.ColoredWriteLine(ConsoleColor.Green);

    /// <summary>
    /// Writes red text to the console.
    /// </summary>
    /// <param name="text">The text.</param>
    [DebuggerStepThrough]
    public static void RedWriteLine(this string text) => text.ColoredWriteLine(ConsoleColor.Red);

    /// <summary>
    /// Writes yellow text to the console.
    /// </summary>
    /// <param name="text">The text.</param>
    [DebuggerStepThrough]
    public static void YellowWriteLine(this string text) => text.ColoredWriteLine(ConsoleColor.Yellow);


    /// <summary>
    /// Writes out text with the specified ConsoleColor.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="color">The color.</param>
    [DebuggerStepThrough]
    // ReSharper disable once MemberCanBePrivate.Global
    public static void ColoredWriteLine(this string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
