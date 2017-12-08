using System.Windows;
using View4Logs.UI.Base;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class SideBar : Base.Component<SideBarView, SideBarViewModel>
    {
        private static readonly DependencyPropertyKey SearchPanelEnabledPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SearchPanelEnabled), typeof(bool), typeof(SideBar), new PropertyMetadata());
        public static readonly DependencyProperty SearchPanelEnabledProperty = SearchPanelEnabledPropertyKey.DependencyProperty;
        public bool SearchPanelEnabled
        {
            get => (bool)GetValue(SearchPanelEnabledProperty);
            set => SetValue(SearchPanelEnabledPropertyKey, value);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.Bind(SearchPanelEnabledPropertyKey, ViewModel.SearchPanelEnabledProperty);
        }
    }
}
