using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;

namespace View4Logs.Core.Services
{
    public sealed class LogsViewService : ILogsViewService
    {
        private readonly object _thisLock = new object();

        private readonly BehaviorSubject<IList<LogEvent>> _logEvents;
        private readonly BehaviorSubject<LogEvent> _selectedLogEventProperty;
        private LogEvent _lastSelectedLogEvent;

        public LogsViewService(ILogFilterResultsService logFilterResultsService)
        {
            _logEvents = new BehaviorSubject<IList<LogEvent>>(Array.Empty<LogEvent>());
            _selectedLogEventProperty = new BehaviorSubject<LogEvent>(null);

            LogEvents = _logEvents.AsObservable();

            // Subscribe to log filter result
            logFilterResultsService.Result.AsItemsBehaviorObservable().Subscribe(_logEvents);

            LogEvents.Subscribe(CoerceSelectedLogEvent);

            SelectedLogEventProperty = _selectedLogEventProperty
                .DistinctUntilChanged()
                .AsObservable();
        }

        public IObservable<IList<LogEvent>> LogEvents { get; }

        public IObservable<LogEvent> SelectedLogEventProperty { get; }

        public LogEvent SelectedLogEvent
        {
            get => _selectedLogEventProperty.Value;
            set
            {
                if (value == _selectedLogEventProperty.Value)
                {
                    return;
                }

                _selectedLogEventProperty.OnNext(value);
                if (value != null)
                {
                    _lastSelectedLogEvent = value;
                }
            }
        }

        public void SelectNext()
        {
            Move(1);
        }

        public void SelectPrevious()
        {
            Move(-1);
        }

        private void Move(int step)
        {
            lock (_thisLock)
            {
                if (SelectedLogEvent != null)
                {
                    var logEvents = _logEvents.Value;
                    var i = logEvents.IndexOf(SelectedLogEvent) + step;
                    SelectedLogEvent = _logEvents.Value[i % logEvents.Count];
                }
            }
        }

        private void CoerceSelectedLogEvent(IList<LogEvent> logEvents)
        {
            lock (_thisLock)
            {
                if (logEvents != null && _lastSelectedLogEvent != null)
                {
                    if (logEvents.Contains(_lastSelectedLogEvent))
                    {
                        SelectedLogEvent = _lastSelectedLogEvent;
                    }
                    else
                    {
                        SelectedLogEvent = null;
                    }
                }
            }
        }
    }
}
