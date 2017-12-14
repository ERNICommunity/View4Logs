using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using View4Logs.Common.Collections;
using View4Logs.Common.Data;
using View4Logs.Common.Interfaces;
using View4Logs.UI.Base;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.Services
{
    public sealed class LogsViewService : DispatcherObject, ILogsViewService
    {
        private readonly BehaviorSubject<IList<LogEvent>> _logEvents;
        private readonly BehaviorSubject<LogEvent> _selectedLogEventProperty;
        private LogEvent _lastSelectedLogEvent;
        private IScrollInfo _scrollInfo;

        public LogsViewService(ILogFilterResultsService logFilterResultsService, ILayoutService layoutService)
        {
            _logEvents = new BehaviorSubject<IList<LogEvent>>(Array.Empty<LogEvent>());
            _selectedLogEventProperty = new BehaviorSubject<LogEvent>(null);

            LogEvents = _logEvents.AsObservable();

            // Subscribe to log filter result
            logFilterResultsService
                .Result
                .AsItemsBehaviorObservable()
                .ObserveOn(Dispatcher)
                .Subscribe(_logEvents);

            LogEvents.Subscribe(CoerceSelectedLogEvent);

            SelectedLogEventProperty = _selectedLogEventProperty
                .DistinctUntilChanged()
                .AsObservable();

            OpenLogEventCommand = Command.Create(
                SelectedLogEventProperty.Select<LogEvent, Func<object, bool>>(logEvent => param => logEvent != null),
                param =>
                {
                    layoutService.LogEventDetailVisibleProperty.OnNext(true);
                }
            );

            HideLogEventCommand = Command.Create(
                (object param) => layoutService.LogEventDetailVisibleProperty.OnNext(false)
            );

            SelectNextCommand = Command.Create((object o) => Move(1));
            SelectPreviousCommand = Command.Create((object o) => Move(-1));
            SelectNextPageCommand = Command.Create((object o) => NextPage());
            SelectPreviousPageCommand = Command.Create((object o) => PreviousPage());
            SelectFirstCommand = Command.Create((object o) => SelectFirst());
            SelectLastCommand = Command.Create((object o) => SelectLast());
        }

        private void NextPage()
        {
            var pageSize = PageSize;
            var offset = SelectedIndex - PageOffset;

            var step = pageSize - (offset % pageSize) - 1;
            if (step == 0)
            {
                step = pageSize;
            }

            Move(step);
        }

        private void PreviousPage()
        {
            var pageSize = PageSize;
            var offset = SelectedIndex - PageOffset;

            var step = -(offset % pageSize);
            if (step == 0)
            {
                step = -pageSize;
            }

            Move(step);
        }

        private int PageSize => (int)(_scrollInfo?.ViewportHeight ?? 0);

        private int PageOffset => (int)(_scrollInfo?.VerticalOffset ?? 0);

        private int SelectedIndex => SelectedLogEvent != null
            ? _logEvents.Value.IndexOf(SelectedLogEvent)
            : -1;

        public IObservable<IList<LogEvent>> LogEvents { get; }

        public IObservable<LogEvent> SelectedLogEventProperty { get; }

        public ICommand OpenLogEventCommand { get; }

        public ICommand HideLogEventCommand { get; }

        public ICommand SelectNextCommand { get; }

        public ICommand SelectPreviousCommand { get; }

        public ICommand SelectFirstCommand { get; }

        public ICommand SelectLastCommand { get; }

        public ICommand SelectNextPageCommand { get; }

        public ICommand SelectPreviousPageCommand { get; }

        public void SetScrollhandle(IScrollInfo scrollInfo)
        {
            _scrollInfo = scrollInfo;
        }

        public LogEvent SelectedLogEvent
        {
            get => _selectedLogEventProperty.Value;
            set
            {
                CheckAccess();

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

        private void Move(int step)
        {
            CheckAccess();

            if (SelectedLogEvent != null)
            {
                var logEvents = _logEvents.Value;
                var i = SelectedIndex + step;

                if (i < 0)
                {
                    i = 0;
                }
                else if (i > logEvents.Count - 1)
                {
                    i = logEvents.Count - 1;
                }

                SelectedLogEvent = _logEvents.Value[i];
            }
            else
            {
                SelectedLogEvent = _logEvents.Value.FirstOrDefault();
            }
        }

        private void CoerceSelectedLogEvent(IList<LogEvent> logEvents)
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

        private void SelectFirst()
        {
            CheckAccess();

            if (_logEvents.Value.Count > 0)
            {
                SelectedLogEvent = _logEvents.Value[0];
            }
        }

        private void SelectLast()
        {
            CheckAccess();

            var logEvents = _logEvents.Value;
            if (logEvents.Count > 0)
            {
                SelectedLogEvent = logEvents[logEvents.Count - 1];
            }
        }
    }
}
