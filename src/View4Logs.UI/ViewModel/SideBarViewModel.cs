using System;
using System.Reactive.Linq;
using System.Windows.Input;
using View4Logs.UI.Base;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;

namespace View4Logs.UI.ViewModel
{
    public sealed class SideBarViewModel : Base.ViewModel
    {
        public SideBarViewModel(IDialogService dialogService)
        {
            SearchPanelEnabledProperty = CreateProperty<bool>(nameof(SearchPanelEnabled));

            OpenAppSettingsDialog = Command.Create<object>(o => dialogService.ShowDialog(new AppSettingsDialog()));
        }

        public ObservableProperty<bool> SearchPanelEnabledProperty { get; }
        public bool SearchPanelEnabled
        {
            get => SearchPanelEnabledProperty.Value;
            set => SearchPanelEnabledProperty.Value = value;
        }

        public ICommand OpenAppSettingsDialog { get; }
    }
}
