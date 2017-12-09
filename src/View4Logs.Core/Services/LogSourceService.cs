using System;
using System.Collections.Generic;
using System.Linq;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Core.Services
{
    public sealed class LogSourceService : ILogSourceService, IDisposable
    {
        private readonly object _logEventsLock;
        private readonly object _sourcesLock;
        private readonly ObservableCowList<ILogSource> _sources;
        private readonly ObservableCowList<LogEvent> _logEvents;

        public LogSourceService()
        {
            _logEventsLock = new object();
            _sourcesLock = new object();
            _sources = new ObservableCowList<ILogSource>();
            _logEvents = new ObservableCowList<LogEvent>();

            Sources = _sources;
            LogEvents = _logEvents;
        }

        public INotifyListChanged<LogEvent> LogEvents { get; }

        public INotifyListChanged<ILogSource> Sources { get; }

        public void AddSource(ILogSource source)
        {
            lock (_sourcesLock)
            {
                _sources.Add(source);

                source.LogEvents.Subscribe(
                    Append,
                    () => _sources.Remove(source)
                );

                source.Reset.Subscribe(ResetSource);
            }
        }

        public void ResetSource(ILogSource source)
        {
            lock (_logEventsLock)
            {
                var logEvents = _logEvents.Where(logEvent => logEvent.Source != source).ToList();
                _logEvents.Reset(logEvents);
            }
        }

        private void Append(IList<LogEvent> logEvents)
        {
            lock (_logEventsLock)
            {
                var needSort = logEvents.Zip(logEvents.Skip(1), (a, b) => a.TimeStamp < b.TimeStamp).Contains(false);
                if (needSort)
                {
                    logEvents = logEvents.OrderBy(logEvent => logEvent.TimeStamp).ToList();
                }

                _logEvents.Add(logEvents);
            }
        }

        public void Clear()
        {
            lock (_sourcesLock)
            {
                lock (_logEventsLock)
                {
                    foreach (var src in _sources)
                    {
                        src.Dispose();
                    }

                    _sources.Clear();
                    _logEvents.Clear();
                }
            }
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
