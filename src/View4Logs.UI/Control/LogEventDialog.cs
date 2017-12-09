using System.Reactive;
using View4Logs.Common.Data;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class LogEventDialog : Dialog<LogEventDialogView, LogEventDialogViewModel, Unit>
    {
        public LogEvent LogEvent { get; }

        public LogEventDialog(LogEvent logEvent)
        {
            LogEvent = logEvent;
        }

        protected override LogEventDialogViewModel ViewModelFactory()
        {
            return new LogEventDialogViewModel(LogEvent);
        }
    }
}
