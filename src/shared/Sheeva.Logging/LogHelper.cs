namespace Sheeva.Logging;

public static class LogHelper
{
    public static void InitializeLog() =>
        Serilog.Log.Information("Starting up");

    public static void CloseLog()
    {
        Serilog.Log.Information("Shut down complete");
        Serilog.Log.CloseAndFlush();
    }

    public static void LogException(Exception ex) => Serilog.Log.Fatal(ex, "Unhandled exception");
}
