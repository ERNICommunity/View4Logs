using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Subjects;
using View4Logs.Common;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.Common.Collections;

namespace View4Logs.Core.Filters
{
    public sealed class LogSourceLevelFilter : ILogSourceLevelFilter, IDisposable
    {
        private readonly object _thisLock;
        private readonly Dictionary<ILogSource, LogLevel> _sourceLevels;
        private readonly BehaviorSubject<Func<LogEvent, bool>> _filter;
        private readonly IDisposable _sourcesSubscription;

        public LogSourceLevelFilter(ILogSourceService logSourceService, ILogFilterService logFilterService)
        {
            _thisLock = new object();
            _sourceLevels = new Dictionary<ILogSource, LogLevel>();
            _filter = new BehaviorSubject<Func<LogEvent, bool>>(LogFilter.PassAll);

            logFilterService.AddFilter(_filter);

            _sourcesSubscription = logSourceService.Sources.AsItemsBehaviorObservable().Subscribe(OnSourcesChanged);
        }

        public void SetLogLevelForSource(ILogSource source, LogLevel level)
        {
            Dictionary<ILogSource, LogLevel> souceLevelsSafeCopy = null;

            lock (_thisLock)
            {
                if (level == LogLevel.All)
                {
                    _sourceLevels.Remove(source);
                }
                else
                {
                    _sourceLevels[source] = level;
                }

                souceLevelsSafeCopy = new Dictionary<ILogSource, LogLevel>(_sourceLevels);
            }

            UpdateFilter(souceLevelsSafeCopy);
        }

        public LogLevel GetLogLevelForSource(ILogSource source)
        {
            lock (_thisLock)
            {
                return _sourceLevels.TryGetValue(source, out var level) ? level : LogLevel.All;
            }
        }

        private void OnSourcesChanged(IList<ILogSource> sources)
        {
            Dictionary<ILogSource, LogLevel> souceLevelsSafeCopy = null;

            lock (_thisLock)
            {
                var removed = _sourceLevels.Keys.Where(source => !sources.Contains(source)).ToList();
                foreach (var key in removed)
                {
                    _sourceLevels.Remove(key);
                }

                souceLevelsSafeCopy = new Dictionary<ILogSource, LogLevel>(_sourceLevels);
            }

            UpdateFilter(souceLevelsSafeCopy);
        }

        private void UpdateFilter(Dictionary<ILogSource, LogLevel> sourceLevels)
        {
            if (sourceLevels.Count == 0)
            {
                _filter.OnNext(LogFilter.PassAll);
                return;
            }

            bool Filter(LogEvent logEvent)
            {
                if (sourceLevels.TryGetValue(logEvent.Source, out var level))
                {
                    return logEvent.Level >= level;
                }

                return true;
            }

            _filter.OnNext(Filter);
        }

        public void Dispose()
        {
            _sourcesSubscription.Dispose();
            _filter.OnCompleted();
            _filter.Dispose();
        }
    }
}
