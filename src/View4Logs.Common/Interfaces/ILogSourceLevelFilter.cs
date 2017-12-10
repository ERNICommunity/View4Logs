using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogSourceLevelFilter
    {
        void SetLogLevelForSource(ILogSource source, LogLevel level);

        LogLevel GetLogLevelForSource(ILogSource source);
    }
}
