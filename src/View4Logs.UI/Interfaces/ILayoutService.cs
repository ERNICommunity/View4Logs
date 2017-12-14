using System;
using System.Reactive.Subjects;

namespace View4Logs.UI.Interfaces
{
    public interface ILayoutService
    {
        BehaviorSubject<bool> SearchPanelVisibleProperty { get; }

        BehaviorSubject<bool> LogSourcesVisibleProperty { get; }

        BehaviorSubject<bool> LoggersTreeVisibleProperty { get; }

        BehaviorSubject<bool> LogEventDetailVisibleProperty { get; }

        BehaviorSubject<bool> TimelineVisibleProperty { get; }

        IObservable<bool> LeftPanelVisibleProperty { get; }
    }
}