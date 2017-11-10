﻿using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils;

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
        }

        
        public IList<LogMessage> Messages => _messages.Value;
    }
}
