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
        private readonly BehaviorSubject<Func<LogMessage, bool>> _filter;
        private readonly ObservableCowList<IObservable<Func<LogMessage, bool>>> _filters;

        public LogFilterService()
        {
            _filter = new BehaviorSubject<Func<LogMessage, bool>>(LogFilter.PassAll);
            _filters = new ObservableCowList<IObservable<Func<LogMessage, bool>>>();

            _filters
                .AsItemsBehaviorObservable()
                .Select(items => items.CombineLatest(CombineFilters))
                .Switch()
                .DistinctUntilChanged()
                .Subscribe(_filter);

            Filter = _filter.AsObservable();
        }

        public IObservable<Func<LogMessage, bool>> Filter { get; }

        public void AddFilter(IObservable<Func<LogMessage, bool>> filter)
        {
            lock (_filtersLock)
            {
                _filters.Add(filter);
            }
        }

        private Func<LogMessage, bool> CombineFilters(IList<Func<LogMessage, bool>> filters)
        {
            var activeFilters = filters.Where(f => f != LogFilter.PassAll).ToArray();

            if (activeFilters.Length == 0)
            {
                return LogFilter.PassAll;
            }

            return msg => Array.TrueForAll(activeFilters, f => f(msg));
        }

        public void Dispose()
        {
            _filter.Dispose();
        }
    }
}
