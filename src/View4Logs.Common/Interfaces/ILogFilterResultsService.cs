using View4Logs.Common.Collections;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogFilterResultsService
    {
        INotifyListChanged<LogEvent> Result { get; }
    }
}
