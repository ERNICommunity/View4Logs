using System.Windows;
using Autofac;

namespace View4Logs.UI.Control
{
    public sealed class AppWindow : Window
    {
        public AppWindow(ILifetimeScope scope)
        {
            WindowState = WindowState.Maximized;
            Title = "View4Logs";
            Content = scope.Resolve<App>();
        }
    }
}