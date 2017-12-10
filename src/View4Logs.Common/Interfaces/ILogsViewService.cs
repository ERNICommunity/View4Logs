using System;
using System.Collections.Generic;
using View4Logs.Common.Data;

namespace View4Logs.Common.Interfaces
{
    public interface ILogsViewService
    {
        IObservable<IList<LogEvent>> LogEvents { get; }

        LogEvent SelectedLogEvent { get; set; }

        IObservable<LogEvent> SelectedLogEventProperty { get; }

        void SelectNext();

        void SelectPrevious();
    }
}
