using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Core.Services
{
    public sealed class LogFilterResultsService : ILogFilterResultsService
    {
        private readonly ObservableCowList<LogEvent> _logEvents;

        public LogFilterResultsService(ILogSourceService logSourceService, ILogFilterService logFilterService)
        {
            _logEvents = new ObservableCowList<LogEvent>();
            Result = _logEvents;

            var sourceLogEvents = logSourceService.LogEvents.AsBehaviorObservable().Publish();
            var filterChanges = logFilterService.Filter.Publish();
            var sourceResetEvents = sourceLogEvents.Where(e => e.Action == NotifyListChangedAction.Reset).Select(e => e.Items);

            Observable.Merge(
                Observable.WithLatestFrom(
                    sourceResetEvents,
                    filterChanges,
                    (items, filter) => (items, filter)
                ),
                Observable.WithLatestFrom(
                    filterChanges,
                    sourceLogEvents.Select(e => e.Items),
                    (filter, items) => (items, filter)
                )
            )
            .Select(x =>
            {
                (var items, var filter) = x;
                return Observable.Concat(
                    InvokeFilter(NotifyListChangedAction.Reset, items, filter),
                    sourceLogEvents
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
                        _logEvents.Add(items);
                        break;
                    case NotifyListChangedAction.Reset:
                        _logEvents.Reset(items);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });

            filterChanges.Connect();
            sourceLogEvents.Connect();
        }

        public INotifyListChanged<LogEvent> Result { get; }

        private IObservable<(NotifyListChangedAction, IList<LogEvent>)> InvokeFilter(NotifyListChangedAction action, IList<LogEvent> logEvents, Func<LogEvent, bool> filter)
        {
            return Observable.StartAsync(token => Task.Run(() => (action, ApplyFilter(logEvents, filter, token)), token));
        }

        private IList<LogEvent> ApplyFilter(IList<LogEvent> logEvents, Func<LogEvent, bool> filter, CancellationToken token)
        {
            return logEvents
                    .AsParallel()
                    .AsOrdered()
                    .WithCancellation(token)
                    .Where(filter)
                    .ToList();
        }
    }
}
