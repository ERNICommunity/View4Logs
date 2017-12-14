using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public sealed class AppViewModel : Base.ViewModel
    {
        private readonly ObservableProperty<bool> _leftPanelVisible;
        private readonly ObservableProperty<bool> _logSourcesVisible;
        private readonly ObservableProperty<bool> _searchPanelVisible;
        private readonly ObservableProperty<bool> _logEventDetailVisible;
        private readonly ObservableProperty<bool> _timelineVisible;

        public AppViewModel(ILayoutService layoutService)
        {
            _leftPanelVisible = CreateProperty<bool>(nameof(LeftPanelVisible), layoutService.LeftPanelVisibleProperty);
            _logSourcesVisible = CreateProperty<bool>(nameof(LogSourcesVisible), layoutService.LogSourcesVisibleProperty);
            _searchPanelVisible = CreateProperty<bool>(nameof(SearchPanelVisible), layoutService.SearchPanelVisibleProperty);
            _logEventDetailVisible = CreateProperty<bool>(nameof(LogEventDetailVisible), layoutService.LogEventDetailVisibleProperty);
            _timelineVisible = CreateProperty<bool>(nameof(TimeLineVisible), layoutService.TimelineVisibleProperty);
        }

        public bool LeftPanelVisible => _leftPanelVisible.Value;

        public bool LogSourcesVisible => _logSourcesVisible.Value;

        public bool SearchPanelVisible => _searchPanelVisible.Value;

        public bool LogEventDetailVisible => _logEventDetailVisible.Value;

        public bool TimeLineVisible => _timelineVisible.Value;
    }
}
