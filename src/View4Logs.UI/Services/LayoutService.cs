using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.Services
{
    public sealed class LayoutService : ILayoutService
    {
        public LayoutService()
        {
            SearchPanelVisibleProperty = new BehaviorSubject<bool>(false);
            LogSourcesVisibleProperty = new BehaviorSubject<bool>(false);
            LoggersTreeVisibleProperty = new BehaviorSubject<bool>(false);
            LogEventDetailVisibleProperty = new BehaviorSubject<bool>(false);
            TimelineVisibleProperty = new BehaviorSubject<bool>(false);

            var leftPanelContent = new[]
            {
                LogSourcesVisibleProperty,
                LoggersTreeVisibleProperty
            };

            LeftPanelVisibleProperty = leftPanelContent.CombineLatest(content => content.Contains(true));

            SetExclusiveVisiblity(leftPanelContent);
        }

        public BehaviorSubject<bool> SearchPanelVisibleProperty { get; }

        public BehaviorSubject<bool> LogSourcesVisibleProperty { get; }

        public BehaviorSubject<bool> LoggersTreeVisibleProperty { get; }

        public BehaviorSubject<bool> LogEventDetailVisibleProperty { get; }

        public BehaviorSubject<bool> TimelineVisibleProperty { get; }

        public IObservable<bool> LeftPanelVisibleProperty { get; }

        private void SetExclusiveVisiblity(IList<BehaviorSubject<bool>> group)
        {
            foreach (var item in group)
            {
                group
                    .Where(x => x != item)      // group without current item
                    .Merge()                    // get notification when any changes
                    .Where(visible => visible)  // ...to visible state
                    .Select(_ => false)         // convert value to false
                    .Subscribe(item);           // hide current item
            }
        }
    }
}