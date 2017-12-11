using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<IList<LogEvent>> _logEvents;
        private readonly ObservableProperty<LogEvent> _selectedLogEvent;

        public LogsViewModel(ILogsViewService logsViewService, ILogFileImportService logFileImportService, Dispatcher dispatcher)
        {
            LogsViewService = logsViewService;

            _logEvents = CreateProperty<IList<LogEvent>>(nameof(LogEvents));
            _selectedLogEvent = CreateProperty<LogEvent>(nameof(SelectedLogEvent));

            logsViewService.LogEvents
                .Subscribe(value =>
                {
                    _logEvents.Value = value;

                    // We need UI to refresh selection
                    RaisePropertyChanged(nameof(SelectedLogEvent));
                });

            logsViewService
                .SelectedLogEventProperty                
                .Subscribe(_selectedLogEvent);

            _selectedLogEvent
                .Where(logEvent => logEvent != null)
                .Subscribe(logEvent => logsViewService.SelectedLogEvent = logEvent);

            OpenFileCommand = Command.Create((string[] files) =>
            {
                foreach (var file in files)
                {
                    logFileImportService.Import(file);
                }
            });

            SetScrollHandleCommand = Command.Create((IScrollInfo param) =>
            {
                logsViewService.SetScrollhandle(param);
            });
        }

        public IList<LogEvent> LogEvents => _logEvents.Value;

        public LogEvent SelectedLogEvent
        {
            get => _selectedLogEvent.Value;
            set => _selectedLogEvent.Value = value;
        }

        public ILogsViewService LogsViewService { get; }

        public ICommand OpenFileCommand { get; }

        public ICommand SetScrollHandleCommand { get; }
    }
}
