using System.Reactive;
using System.Windows.Input;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class MessageDialogViewModel : DialogViewModelBase<Unit>
    {
        public string Title { get; }
        public string Message { get; }

        public MessageDialogViewModel(string title, string message)
        {
            Title = title;
            Message = message;

            CloseCommand = Command.Create((object o) => Close());
        }

        public ICommand CloseCommand { get; }
    }
}
