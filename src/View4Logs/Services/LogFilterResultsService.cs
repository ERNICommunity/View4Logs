using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Utils.Collections;

namespace View4Logs.Services
{
    public sealed class LogFilterResultsService : ILogFilterResultsService
    {
        private readonly ObservableCowList<LogMessage> _messages;

        public LogFilterResultsService(ILogSourceService logSourceService, ILogFilterService logFilterService)
        {
            _messages = new ObservableCowList<LogMessage>();
            Messages = _messages;

            var sourceMessages = logSourceService.Messages.AsBehaviorObservable().Publish();
            var filterChanges = logFilterService.Filter.Publish();
            var sourceResetMessages = sourceMessages.Where(e => e.Action == NotifyListChangedAction.Reset).Select(e => e.Items);

            Observable.Merge(
                Observable.WithLatestFrom(
                    sourceResetMessages,
                    filterChanges,
                    (items, filter) => (items, filter)
                ),
                Observable.WithLatestFrom(
                    filterChanges,
                    sourceMessages.Select(e => e.Items),
                    (filter, items) => (items, filter)
                )
            )
            .Select(x =>
            {
                (var items, var filter) = x;
                return Observable.Concat(
                    InvokeFilter(NotifyListChangedAction.Reset, items, filter),
                    sourceMessages
                        .TakeWhile(e => e.Action == NotifyListChangedAction.Add)
                        .Select(e => InvokeFilter(e.Action, e.NewItems, filter))
                        .SelectMany(a => a)
                );
            })
            .Switch().Subscribe(x =>
            {
                (var action, var items) = x;
                switch (action)
                {
                    case NotifyListChangedAction.Add:
                        _messages.Add(items);
                        break;
                    case NotifyListChangedAction.Reset:
                        _messages.Reset(items);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            filterChanges.Connect();
            sourceMessages.Connect();
        }

        public INotifyListChanged<LogMessage> Messages { get; }

        private IObservable<(NotifyListChangedAction, IList<LogMessage>)> InvokeFilter(NotifyListChangedAction action, IList<LogMessage> messages, Func<LogMessage, bool> filter)
        {
            return Observable.StartAsync(token => Task.Run(() => (action, ApplyFilter(messages, filter, token)), token));
        }

        private IList<LogMessage> ApplyFilter(IList<LogMessage> messages, Func<LogMessage, bool> filter, CancellationToken token)
        {
            return messages
                    .AsParallel()
                    .AsOrdered()
                    .WithCancellation(token)
                    .Where(filter)
                    .ToList();
        }
    }
}
