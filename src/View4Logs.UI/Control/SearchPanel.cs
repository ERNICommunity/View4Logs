using System;
using System.Windows;
using System.Windows.Controls;
using View4Logs.UI.Base;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class SearchPanel : Component<SearchPanelView, SearchPanelViewModel>
    {
        public SearchPanel()
        {
            // TODO: temporary hack to focus search bar when panel becomes visible
            this.PropertyAsObservable<Visibility>(VisibilityProperty).Subscribe(visibility =>
            {
                if (visibility == Visibility.Visible)
                {
                    (View.FindName("SearchBox") as TextBox)?.Focus();
                }
            });
        }

        protected override void OnLoaded()
        {
            
        }
    }
}
