using System;
using System.Reactive;
using System.Windows.Input;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public class LogEventDialogViewModel : DialogViewModelBase<Unit>
    {
        private readonly ObservableProperty<LogEvent> _logEvent;
        private readonly IDisposable _selectedLogEventSubscription;

        public LogEventDialogViewModel(ILogsViewService logsViewService, ITextSelectionProvider textSelectionProvider, IWebSearchService webSearchService)
        {
            _logEvent = CreateProperty<LogEvent>(nameof(LogEvent));

            _selectedLogEventSubscription = logsViewService.SelectedLogEventProperty.Subscribe(_logEvent);

            CloseCommand = Command.Create((object o) => Return(Unit.Default));

            WebSearchCommand = Command.Create((object o) =>
            {
                var text = textSelectionProvider.GetSelectedText();
                if (text != null)
                {
                    webSearchService.OpenWebSearch(text);
                }
            });

            SelectNextCommand = Command.Create((object o) => logsViewService.SelectNext());
            SelectPreviousCommand = Command.Create((object o) => logsViewService.SelectPrevious());
        }

        public LogEvent LogEvent => _logEvent.Value;

        public ICommand CloseCommand { get; }

        public ICommand WebSearchCommand { get; }

        public ICommand SelectNextCommand { get; }

        public ICommand SelectPreviousCommand { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _selectedLogEventSubscription.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
