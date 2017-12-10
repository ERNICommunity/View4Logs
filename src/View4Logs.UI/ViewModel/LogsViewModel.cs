using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<IList<LogEvent>> _logEvents;
        private readonly ObservableProperty<LogEvent> _selectedLogEvent;

        public LogsViewModel(ILogsViewService logsViewService, ILogFileImporter logFileImporter, IDialogService dialogService)
        {
            _logEvents = CreateProperty<IList<LogEvent>>(nameof(LogEvents));
            _selectedLogEvent = CreateProperty<LogEvent>(nameof(SelectedLogEvent));

            logsViewService.LogEvents
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(_logEvents);

            logsViewService.SelectedLogEventProperty.Subscribe(_selectedLogEvent);
            _selectedLogEvent
                .Where(logEvent => logEvent != null)
                .Subscribe(LogEvent => logsViewService.SelectedLogEvent = LogEvent);

            OpenFileCommand = Command.Create(async (string[] files) =>
            {
                await Task.Run(() => logFileImporter.Import(files[0]));
            });

            OpenLogEventCommand = Command.Create(async (LogEvent logEvent) =>
            {
                logsViewService.SelectedLogEvent = logEvent;
                await dialogService.ShowDialog(new LogEventDialog());
            });
        }

        public IList<LogEvent> LogEvents => _logEvents.Value;

        public LogEvent SelectedLogEvent
        {
            get => _selectedLogEvent.Value;
            set => _selectedLogEvent.Value = value;
        }

        public ICommand OpenFileCommand { get; }

        public ICommand OpenLogEventCommand { get; }
    }
}
