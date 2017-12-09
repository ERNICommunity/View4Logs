using System.Reactive;
using System.Windows.Input;
using View4Logs.Common.Data;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public class LogEventDialogViewModel : DialogViewModelBase<Unit>
    {
        public LogEventDialogViewModel(LogEvent logEvent)
        {
            LogEvent = logEvent;
            CloseCommand = Command.Create((object o) => Return(Unit.Default));
        }

        public LogEvent LogEvent { get; }

        public ICommand CloseCommand { get; }

    }
}
