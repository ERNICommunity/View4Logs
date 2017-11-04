using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Autofac;

namespace View4Logs.Base
{
    public abstract class Component : ContentControl
    {
        public static readonly DependencyProperty ScopeProperty = DependencyProperty.RegisterAttached("Scope", typeof(ILifetimeScope), typeof(Component), new FrameworkPropertyMetadata { Inherits = true });

        public static ILifetimeScope GetScope(Component target)
        {
            return (ILifetimeScope)target.GetValue(ScopeProperty);
        }

        public static void SetScope(Component target, ILifetimeScope value)
        {
            target.SetValue(ScopeProperty, value);
        }

        public ILifetimeScope Scope
        {
            get => (ILifetimeScope)GetValue(ScopeProperty);
            set => SetValue(ScopeProperty, value);
        }
    }

    public abstract class Component<TView, TViewModel> : Component
        where TView : View, IComponentConnector, new()
        where TViewModel : class
    {


        private static readonly Lazy<TView> View = new Lazy<TView>(ViewFactory);

        protected Component()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        protected TViewModel ViewModel { get; private set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ContentTemplate = View.Value.Template;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel = ViewModelFactory();
            Content = ViewModel;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            (ViewModel as IDisposable)?.Dispose();
            Content = ViewModel = null;
        }              

        private static TView ViewFactory()
        {
            var view = new TView();
            view.InitializeComponent();
            return view;
        }

        protected virtual TViewModel ViewModelFactory()
        {
            return Scope.Resolve<TViewModel>();
        }
    }
}
