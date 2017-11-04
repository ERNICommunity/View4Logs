using System.Windows;
using Autofac;

namespace View4Logs.UI.Control
{
    public class AppWindow : Window
    {
        public AppWindow(ILifetimeScope scope)
        {
            WindowState = WindowState.Maximized;
            Content = scope.Resolve<App>();
        }
    }
}