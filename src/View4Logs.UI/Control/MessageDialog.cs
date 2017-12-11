using System.Reactive;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class MessageDialog : Dialog<MessageDialogView, MessageDialogViewModel, Unit>
    {
        public string Title { get; }
        public string Message { get; }

        public MessageDialog(string title, string message)
        {
            Title = title;
            Message = message;
        }

        protected override MessageDialogViewModel ViewModelFactory()
        {
            return new MessageDialogViewModel(Title, Message);
        }
    }
}
