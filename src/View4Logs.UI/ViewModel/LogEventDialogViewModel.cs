using System.Reactive;
using System.Windows.Input;
using View4Logs.Common.Data;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public class LogEventDialogViewModel : DialogViewModelBase<Unit>
    {
        public LogEventDialogViewModel(ITextSelectionProvider textSelectionProvider, IWebSearchService webSearchService)
        {
            CloseCommand = Command.Create((object o) => Return(Unit.Default));

            WebSearch = Command.Create((object o) =>
            {
                var text = textSelectionProvider.GetSelectedText();
                if (text != null)
                {
                    webSearchService.OpenWebSearch(text);
                }
            });
        }

        public LogEvent LogEvent { get; set; }

        public ICommand CloseCommand { get; }

        public ICommand WebSearch { get; }
    }
}
