using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using View4Logs.Common.Data;

namespace View4Logs.UI.Interfaces
{
    public interface ILogsViewService
    {
        IObservable<IList<LogEvent>> LogEvents { get; }

        LogEvent SelectedLogEvent { get; set; }

        IObservable<LogEvent> SelectedLogEventProperty { get; }

        ICommand OpenLogEventCommand { get; }

        ICommand HideLogEventCommand { get; }

        ICommand SelectNextCommand { get; }

        ICommand SelectPreviousCommand { get; }

        ICommand SelectFirstCommand { get; }

        ICommand SelectLastCommand { get; }

        ICommand SelectNextPageCommand { get; }

        ICommand SelectPreviousPageCommand { get; }

        void SetScrollhandle(IScrollInfo scrollInfo);
    }
}
