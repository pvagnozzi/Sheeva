// ReSharper disable UnusedMember.Global
namespace Sheeva.Logging;

using System.Diagnostics;
using Microsoft.Extensions.Logging;

#region Classes
public class LoggerActionBase<TDelegate> where TDelegate : Delegate
{
    private readonly Dictionary<LogLevel, TDelegate> actions;

    private readonly Func<LogLevel, TDelegate> builder;

    protected TDelegate GetAction(LogLevel index)
    {
        if (this.actions.TryGetValue(index, out var result))
        {
            return result;
        }

        result = this.builder(index);
        this.actions.Add(index, result);
        return result;
    }

    protected LoggerActionBase(Func<LogLevel, TDelegate> builder)
    {
        this.actions = new Dictionary<LogLevel, TDelegate>();
        this.builder = builder;
    }
}

public class LoggerAction(EventId eventId, string formatMessage) : LoggerActionBase<Action<ILogger, Exception?>>(logLevel =>
        LoggerMessage.Define(logLevel, eventId, formatMessage))
{
    [DebuggerStepThrough]
    public void Execute(ILogger logger, LogLevel level, Exception? ex = null) => this.GetAction(level)(logger, ex);
}

public class LoggerAction<T1>(EventId eventId, string formatMessage)
    : LoggerActionBase<Action<ILogger, T1, Exception?>>(logLevel =>
        LoggerMessage.Define<T1>(logLevel, eventId, formatMessage))
{
    [DebuggerStepThrough]
    public void Execute(ILogger logger, LogLevel level, T1 val1, Exception? ex = null) => this.GetAction(level)(logger, val1, ex);
}

public class LoggerAction<T1, T2>(EventId eventId, string formatMessage)
    : LoggerActionBase<Action<ILogger, T1, T2, Exception?>>(logLevel =>
        LoggerMessage.Define<T1, T2>(logLevel, eventId, formatMessage))
{
    [DebuggerStepThrough]
    public void Execute(ILogger logger, LogLevel level, T1 val1, T2 val2, Exception? ex = null) => this.GetAction(level)(logger, val1, val2, ex);
}

public class LoggerAction<T1, T2, T3>(EventId eventId, string formatMessage)
    : LoggerActionBase<Action<ILogger, T1, T2, T3, Exception?>>(logLevel =>
        LoggerMessage.Define<T1, T2, T3>(logLevel, eventId, formatMessage))
{
    [DebuggerStepThrough]
    public void Execute(ILogger logger, LogLevel level, T1 val1, T2 val2, T3 val3, Exception? ex = null) => this.GetAction(level)(logger, val1, val2, val3, ex);
}
#endregion

public static class LoggerActionExtensions
{
    #region Log
    [DebuggerStepThrough]
    public static void Log(this ILogger logger, LogLevel level, LoggerAction loggerAction, Exception? ex = null) =>
        loggerAction.Execute(logger, level, ex);

    [DebuggerStepThrough]
    public static void Log<T1>(this ILogger logger, LogLevel level, LoggerAction<T1> loggerAction, T1 val1,
        Exception? ex = null) => loggerAction.Execute(logger, level, val1, ex);

    [DebuggerStepThrough]
    public static void Log<T1, T2>(this ILogger logger, LogLevel level, LoggerAction<T1, T2> loggerAction, T1 val1,
        T2 val2, Exception? ex = null) => loggerAction.Execute(logger, level, val1, val2, ex);

    [DebuggerStepThrough]
    public static void Log<T1, T2, T3>(this ILogger logger, LogLevel level, LoggerAction<T1, T2, T3> loggerAction,
        T1 val1, T2 val2,
        T3 val3, Exception? ex = null) => loggerAction.Execute(logger, level, val1, val2, val3, ex);
    #endregion

    #region LogTrace
    [DebuggerStepThrough]
    public static void LogTrace(this ILogger logger, LoggerAction loggerAction, Exception? ex = null) =>
        logger.Log(LogLevel.Trace, loggerAction, ex);

    [DebuggerStepThrough]
    public static void LogTrace<T1>(this ILogger logger, LoggerAction<T1> loggerAction, T1 val1, Exception? ex = null) =>
        logger.Log(LogLevel.Trace, loggerAction, val1, ex);

    [DebuggerStepThrough]
    public static void LogTrace<T1, T2>(this ILogger logger, LoggerAction<T1, T2> loggerAction, T1 val1,
        T2 val2, Exception? ex = null) => logger.Log(LogLevel.Trace, loggerAction, val1, val2, ex);

    [DebuggerStepThrough]
    public static void LogTrace<T1, T2, T3>(this ILogger logger, LoggerAction<T1, T2, T3> loggerAction,
        T1 val1, T2 val2,
        T3 val3, Exception? ex = null) => logger.Log(LogLevel.Trace, loggerAction, val1, val2, val3, ex);
    #endregion

    #region LogDebug
    [DebuggerStepThrough]
    public static void LogDebug(this ILogger logger, LoggerAction loggerAction, Exception? ex = null) =>
        logger.Log(LogLevel.Debug, loggerAction, ex);

    [DebuggerStepThrough]
    public static void LogDebug<T1>(this ILogger logger, LogLevel level, LoggerAction<T1> loggerAction, T1 val1, Exception? ex = null) =>
        logger.Log(LogLevel.Debug, loggerAction, val1, ex);

    [DebuggerStepThrough]
    public static void LogDebug<T1, T2>(this ILogger logger, LogLevel level, LoggerAction<T1, T2> loggerAction, T1 val1,
        T2 val2, Exception? ex = null) => logger.Log(LogLevel.Debug, loggerAction, val1, val2, ex);

    [DebuggerStepThrough]
    public static void LogDebug<T1, T2, T3>(this ILogger logger, LogLevel level, LoggerAction<T1, T2, T3> loggerAction,
        T1 val1, T2 val2,
        T3 val3, Exception? ex = null) => logger.Log(LogLevel.Debug, loggerAction, val1, val2, val3, ex);
    #endregion

    #region LogInformation
    [DebuggerStepThrough]
    public static void LogInformation(this ILogger logger, LoggerAction loggerAction, Exception? ex = null) =>
        logger.Log(LogLevel.Information, loggerAction, ex);

    [DebuggerStepThrough]
    public static void LogInformation<T1>(this ILogger logger, LoggerAction<T1> loggerAction, T1 val1, Exception? ex = null) =>
        logger.Log(LogLevel.Information, loggerAction, val1, ex);

    [DebuggerStepThrough]
    public static void LogInformation<T1, T2>(this ILogger logger, LoggerAction<T1, T2> loggerAction, T1 val1,
        T2 val2, Exception? ex = null) => logger.Log(LogLevel.Information, loggerAction, val1, val2, ex);

    [DebuggerStepThrough]
    public static void LogInformation<T1, T2, T3>(this ILogger logger, LoggerAction<T1, T2, T3> loggerAction,
        T1 val1, T2 val2,
        T3 val3, Exception? ex = null) => logger.Log(LogLevel.Information, loggerAction, val1, val2, val3, ex);
    #endregion

    #region LogWarning
    [DebuggerStepThrough]
    public static void LogWarning(this ILogger logger, LoggerAction loggerAction, Exception? ex = null) =>
        logger.Log(LogLevel.Warning, loggerAction, ex);

    [DebuggerStepThrough]
    public static void LogWarning<T1>(this ILogger logger, LoggerAction<T1> loggerAction, T1 val1, Exception? ex = null) =>
        logger.Log(LogLevel.Warning, loggerAction, val1, ex);

    [DebuggerStepThrough]
    public static void LogWarning<T1, T2>(this ILogger logger, LoggerAction<T1, T2> loggerAction, T1 val1,
        T2 val2, Exception? ex = null) => logger.Log(LogLevel.Warning, loggerAction, val1, val2, ex);

    [DebuggerStepThrough]
    public static void LogWarning<T1, T2, T3>(this ILogger logger, LoggerAction<T1, T2, T3> loggerAction,
        T1 val1, T2 val2,
        T3 val3, Exception? ex = null) => logger.Log(LogLevel.Warning, loggerAction, val1, val2, val3, ex);
    #endregion

    #region LogError
    [DebuggerStepThrough]
    public static void LogError(this ILogger logger, LoggerAction loggerAction, Exception? ex = null) =>
        logger.Log(LogLevel.Error, loggerAction, ex);

    [DebuggerStepThrough]
    public static void LogError<T1>(this ILogger logger, LoggerAction<T1> loggerAction, T1 val1, Exception? ex = null) =>
        logger.Log(LogLevel.Error, loggerAction, val1, ex);

    [DebuggerStepThrough]
    public static void LogError<T1, T2>(this ILogger logger, LoggerAction<T1, T2> loggerAction, T1 val1,
        T2 val2, Exception? ex = null) => logger.Log(LogLevel.Error, loggerAction, val1, val2, ex);

    [DebuggerStepThrough]
    public static void LogError<T1, T2, T3>(this ILogger logger, LoggerAction<T1, T2, T3> loggerAction,
        T1 val1, T2 val2,
        T3 val3, Exception? ex = null) => logger.Log(LogLevel.Error, loggerAction, val1, val2, val3, ex);
    #endregion

    #region LogCritical
    [DebuggerStepThrough]
    public static void LogCritical(this ILogger logger, LoggerAction loggerAction, Exception? ex = null) =>
        logger.Log(LogLevel.Critical, loggerAction, ex);

    [DebuggerStepThrough]
    public static void LogCritical<T1>(this ILogger logger, LoggerAction<T1> loggerAction, T1 val1, Exception? ex = null) =>
        logger.Log(LogLevel.Critical, loggerAction, val1, ex);

    [DebuggerStepThrough]
    public static void LogCritical<T1, T2>(this ILogger logger, LoggerAction<T1, T2> loggerAction, T1 val1,
        T2 val2, Exception? ex = null) => logger.Log(LogLevel.Critical, loggerAction, val1, val2, ex);

    [DebuggerStepThrough]
    public static void LogCritical<T1, T2, T3>(this ILogger logger, LoggerAction<T1, T2, T3> loggerAction,
        T1 val1, T2 val2,
        T3 val3, Exception? ex = null) => logger.Log(LogLevel.Critical, loggerAction, val1, val2, val3, ex);
    #endregion
}
