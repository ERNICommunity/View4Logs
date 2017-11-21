using View4Logs.Common.Data;
using View4Logs.Utils.Collections;

namespace View4Logs.Common.Interfaces
{
    public interface ILogFilterResultsService
    {
        INotifyListChanged<LogMessage> Messages { get; }
    }
}
