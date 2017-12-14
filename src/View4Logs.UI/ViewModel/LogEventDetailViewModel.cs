using System;
using System.Windows.Input;
using View4Logs.Common.Data;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;
using ILogsViewService = View4Logs.UI.Interfaces.ILogsViewService;

namespace View4Logs.UI.ViewModel
{
    public class LogEventDetailViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<LogEvent> _logEvent;

        public LogEventDetailViewModel(ILogsViewService logsViewServiceService, ITextSelectionProvider textSelectionProvider, IWebSearchService webSearchService)
        {
            LogsViewService = logsViewServiceService;

            _logEvent = CreateProperty<LogEvent>(nameof(LogEvent), logsViewServiceService.SelectedLogEventProperty);

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

        public ICommand WebSearchCommand { get; }
    }
}
