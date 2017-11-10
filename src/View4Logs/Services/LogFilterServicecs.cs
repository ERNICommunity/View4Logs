using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.Common;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Services
{
    public sealed class LogFilterService : ILogFilterService, IDisposable
    {
        private readonly ISubject<Func<LogMessage, bool>> _resultFilter;
        private readonly Dictionary<object, Func<LogMessage, bool>> _filters;
        private readonly object _filtersLock = new object();

        public LogFilterService()
        {
            _resultFilter = new BehaviorSubject<Func<LogMessage, bool>>(LogFilter.PassAll);

            // Storage for all current filters.
            _filters = new Dictionary<object, Func<LogMessage, bool>>();

            Filter = _resultFilter.DistinctUntilChanged();
        }

        public IObservable<Func<LogMessage, bool>> Filter { get; }

        public void AddFilter(IObservable<Func<LogMessage, bool>> filter)
        {
            // Reference to the observable filter is used as a key
            object key = filter;

            filter.Subscribe(
                f => SetFilter(key, f),
                ex => RemoveFilter(key),
                () => RemoveFilter(key)
            );
        }

        public void Dispose()
        {
            _resultFilter.OnCompleted();
        }

        private void SetFilter(object key, Func<LogMessage, bool> filter)
        {
            lock (_filtersLock)
            {
                if (filter == LogFilter.PassAll)
                {
                    _filters.Remove(key);
                }
                else
                {
                    _filters[key] = filter;
                }
                
                UpdateResultFilter();
            }
        }

        private void RemoveFilter(object key)
        {
            lock (_filtersLock)
            {
                _filters.Remove(key);
                UpdateResultFilter();
            }
        }

        private void UpdateResultFilter()
        {
            if (_filters.Count > 0)
            {
                var activeFilters = _filters.Values.ToArray();
                bool Func(LogMessage msg) => Array.TrueForAll(activeFilters, f => f(msg));
                _resultFilter.OnNext(Func);
            }
            else
            {
                _resultFilter.OnNext(LogFilter.PassAll);
            }
        }
    }
}
