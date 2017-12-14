using System.Windows.Input;
using View4Logs.UI.Base;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public sealed class SideBarViewModel : Base.ViewModel
    {
        private readonly ILayoutService _layoutService;

        public SideBarViewModel(ILayoutService layoutService, IDialogService dialogService)
        {
            _layoutService = layoutService;
            SearchPanelVisibleProperty = CreateProperty<bool>(nameof(SearchPanelVisible));
            LogSourcesVisibleProperty = CreateProperty<bool>(nameof(LogSourcesVisible));
            LoggersTreeVisibleProperty = CreateProperty<bool>(nameof(LoggersTreeVisible));
            TimelineVisibleProperty = CreateProperty<bool>(nameof(TimelineVisible));

            _layoutService.SearchPanelVisibleProperty.Subscribe(SearchPanelVisibleProperty);
            _layoutService.LogSourcesVisibleProperty.Subscribe(LogSourcesVisibleProperty);
            _layoutService.LoggersTreeVisibleProperty.Subscribe(LoggersTreeVisibleProperty);
            _layoutService.TimelineVisibleProperty.Subscribe(TimelineVisibleProperty);

            OpenAppSettingsDialog = Command.Create<object>(o => dialogService.ShowDialog(new AppSettingsDialog()));
        }

        public ObservableProperty<bool> SearchPanelVisibleProperty { get; }
        public ObservableProperty<bool> LogSourcesVisibleProperty { get; }
        public ObservableProperty<bool> LoggersTreeVisibleProperty { get; }
        public ObservableProperty<bool> TimelineVisibleProperty { get; }


        public bool SearchPanelVisible
        {
            get => SearchPanelVisibleProperty.Value;
            set => _layoutService.SearchPanelVisibleProperty.OnNext(value);
        }

        public bool LogSourcesVisible
        {
            get => LogSourcesVisibleProperty.Value;
            set => _layoutService.LogSourcesVisibleProperty.OnNext(value);
        }

        public bool LoggersTreeVisible
        {
            get => LoggersTreeVisibleProperty.Value;
            set => _layoutService.LoggersTreeVisibleProperty.OnNext(value);
        }

        public bool TimelineVisible
        {
            get => TimelineVisibleProperty.Value;
            set => _layoutService.TimelineVisibleProperty.OnNext(value);
        }

        public ICommand OpenAppSettingsDialog { get; }
    }
}
