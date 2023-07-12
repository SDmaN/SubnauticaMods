using BepInEx.Logging;
using System;

namespace PowerOrder;

internal static class Logger
{
    private static ManualLogSource _logSource;

    public static void SetLogSource(ManualLogSource logSource) => _logSource = logSource;

    public static void Log(LogLevel logLevel, string message) => _logSource.Log(logLevel, message);

    public static void Info(string message) => _logSource.LogInfo(message);

    public static void Error(Exception e) => _logSource.LogError(e);
}
