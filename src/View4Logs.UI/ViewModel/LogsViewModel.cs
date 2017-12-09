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

        public LogsViewModel(ILogFilterResultsService logFilterResultsService, ILogFileImporter logFileImporter, IDialogService dialogService)
        {
            _logEvents = CreateProperty<IList<LogEvent>>(nameof(LogEvents));

            logFilterResultsService.Result.AsItemsBehaviorObservable()
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(_logEvents);

            OpenFileCommand = Command.Create(async (string[] files) =>
            {
                await Task.Run(() => logFileImporter.Import(files[0]));
            });

            OpenLogEventCommand = Command.Create(async (LogEvent logEvent) =>
            {
                await dialogService.ShowDialog(new LogEventDialog(logEvent));
            });
        }

        public IList<LogEvent> LogEvents => _logEvents.Value;

        public ICommand OpenFileCommand { get; }

        public ICommand OpenLogEventCommand { get; }
    }
}
