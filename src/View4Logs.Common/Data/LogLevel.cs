using System;

namespace View4Logs.Common.Data
{
    public enum LogLevel : byte
    {
        All,
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
        Off = byte.MaxValue,
    }

    public static class LogLevels
    {
        public static LogLevel[] All { get; } = (LogLevel[])Enum.GetValues(typeof(LogLevel));
    }
}
