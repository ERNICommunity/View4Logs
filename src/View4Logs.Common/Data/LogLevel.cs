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
}
