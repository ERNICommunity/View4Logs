using View4Logs.Common.Collections;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogSourceService
    {
        INotifyListChanged<LogEvent> LogEvents { get; }

        INotifyListChanged<ILogSource> Sources { get; }

        void AddSource(ILogSource source);

        void Clear();
    }
}
