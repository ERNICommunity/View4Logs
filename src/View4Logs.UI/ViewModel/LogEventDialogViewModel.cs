using System.Reactive;
using System.Windows.Input;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public class LogEventDialogViewModel : DialogViewModelBase<Unit>
    {
        public LogEventDialogViewModel()
        {
            CloseCommand = Command.Create((object o) => Return(Unit.Default));
        }
        public ICommand CloseCommand { get; }
    }
}