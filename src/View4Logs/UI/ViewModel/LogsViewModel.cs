using System;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.Reactive.Concurrency;
using System.Windows.Data;
using View4Logs.Common;
using View4Logs.Services;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsViewModel : Base.ViewModel
    {
        private readonly ILogSourceService _logSourceService;
        private ObservableCollection<LogMessage> _logMessages;

        public LogsViewModel(ILogSourceService logSourceService)
        {
            _logSourceService = logSourceService;
            _logMessages = new ObservableCollection<LogMessage>();

            Messages = new CollectionView(_logMessages);

            _logSourceService.Messages
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(msg => _logMessages.Add(msg));
        }

        public CollectionView Messages { get; }
    }
}
