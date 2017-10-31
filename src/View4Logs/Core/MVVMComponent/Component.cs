using System;
using System.Windows.Controls;
using System.Windows.Markup;

namespace View4Logs.Core.MVVMComponent
{
    public abstract class Component<TView, TViewModel> : ContentControl
        where TView : View, IComponentConnector, new()
    {
        private static readonly Lazy<TView> _view = new Lazy<TView>(ViewFactory);

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ViewModel = ViewModelFactory();
            ContentTemplate = _view.Value.Template;
            Content = ViewModel;
        }        

        protected TViewModel ViewModel { get; private set; }

        protected abstract TViewModel ViewModelFactory();

        private static TView ViewFactory()
        {
            var view = new TView();
            view.InitializeComponent();
            return view;
        }
    }
}
