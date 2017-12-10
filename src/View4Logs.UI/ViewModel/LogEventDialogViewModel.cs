using System;
using System.Reactive;
using System.Windows.Input;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;
using ILogsViewService = View4Logs.UI.Interfaces.ILogsViewService;

namespace View4Logs.UI.ViewModel
{
    public class LogEventDialogViewModel : DialogViewModelBase<Unit>
    {
        private readonly ObservableProperty<LogEvent> _logEvent;
        private readonly IDisposable _selectedLogEventSubscription;

        public LogEventDialogViewModel(ILogsViewService logsViewServiceService, ITextSelectionProvider textSelectionProvider, IWebSearchService webSearchService)
        {
            LogsViewService = logsViewServiceService;

            _logEvent = CreateProperty<LogEvent>(nameof(LogEvent));
            _selectedLogEventSubscription = logsViewServiceService.SelectedLogEventProperty.Subscribe(_logEvent);

            CloseCommand = Command.Create((object o) => Return(Unit.Default));

            WebSearchCommand = Command.Create((object o) =>
            {
                var text = textSelectionProvider.GetSelectedText();
                if (text != null)
                {
                    webSearchService.OpenWebSearch(text);
                }
            });
        }

        public LogEvent LogEvent => _logEvent.Value;

        public ILogsViewService LogsViewService { get; }

        public ICommand CloseCommand { get; }

        public ICommand WebSearchCommand { get; }

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
