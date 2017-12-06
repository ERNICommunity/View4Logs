﻿using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;

namespace View4Logs.UI.ViewModel
{
    public sealed class LogsViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<IList<LogMessage>> _messages;

        public LogsViewModel(ILogFilterResultsService logFilterResultsService, ILogFileImporter logFileImporter)
        {
            _messages = CreateProperty<IList<LogMessage>>(nameof(Messages));

            logFilterResultsService.Messages.AsItemsBehaviorObservable()
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(_messages);

            OpenFileCommand = Command.Create(async (string[] files) =>
            {
                await Task.Run(() => logFileImporter.Import(files[0]));
            });
        }

        public IList<LogMessage> Messages => _messages.Value;

        public ICommand OpenFileCommand { get; }
    }
}