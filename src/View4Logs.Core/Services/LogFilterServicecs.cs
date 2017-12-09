using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.Common;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Core.Services
{
    public sealed class LogFilterService : ILogFilterService, IDisposable
    {
        private readonly object _filtersLock = new object();
        private readonly BehaviorSubject<Func<LogEvent, bool>> _filter;
        private readonly ObservableCowList<IObservable<Func<LogEvent, bool>>> _filters;

        public LogFilterService()
        {
            _filter = new BehaviorSubject<Func<LogEvent, bool>>(LogFilter.PassAll);
            _filters = new ObservableCowList<IObservable<Func<LogEvent, bool>>>();

            _filters
                .AsItemsBehaviorObservable()
                .Select(items => items.CombineLatest(CombineFilters))
                .Switch()
                .DistinctUntilChanged()
                .Subscribe(_filter);

            Filter = _filter.AsObservable();
        }

        public IObservable<Func<LogEvent, bool>> Filter { get; }

        public void AddFilter(IObservable<Func<LogEvent, bool>> filter)
        {
            lock (_filtersLock)
            {
                _filters.Add(filter);
            }
        }

        private Func<LogEvent, bool> CombineFilters(IList<Func<LogEvent, bool>> filters)
        {
            var activeFilters = filters.Where(f => f != LogFilter.PassAll).ToArray();

            if (activeFilters.Length == 0)
            {
                return LogFilter.PassAll;
            }

            return logEvent => Array.TrueForAll(activeFilters, f => f(logEvent));
        }

        public void Dispose()
        {
            _filter.Dispose();
        }
    }
}
