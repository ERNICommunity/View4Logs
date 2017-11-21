using View4Logs.Common.Data;
using View4Logs.Utils.Collections;

namespace View4Logs.Common.Interfaces
{
    public interface ILogSourceService
    {
        INotifyListChanged<LogMessage> Messages { get; }

        INotifyListChanged<ILogSource> Sources { get; }

        void AddSource(ILogSource source);

        void Clear();
    }
}
