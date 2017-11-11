using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Windows.Input;
using View4Logs.Base;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils.Observables;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<IList<LogMessage>> _messages;

        public LogsViewModel(ILogFilterResultsService logFilterResultsService)
        {
            _messages = CreateProperty<IList<LogMessage>>(nameof(Messages));

            logFilterResultsService.Messages
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(_messages);

            OpenFileCommand = Command.Create( async (string[] files) =>
            {
                // TODO
                await Task.Delay(500);
            });
        }

        
        public IList<LogMessage> Messages => _messages.Value;

        public ICommand OpenFileCommand { get; }
    }
}
