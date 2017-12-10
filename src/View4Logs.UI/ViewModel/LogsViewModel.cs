using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;
using ILogsViewService = View4Logs.UI.Interfaces.ILogsViewService;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<IList<LogEvent>> _logEvents;
        private readonly ObservableProperty<LogEvent> _selectedLogEvent;

        public LogsViewModel(ILogsViewService logsViewService, ILogFileImporter logFileImporter)
        {
            LogsViewService = logsViewService;

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
