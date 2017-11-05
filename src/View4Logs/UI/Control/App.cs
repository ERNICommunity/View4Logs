using Autofac;
using View4Logs.Base;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class App : Component<AppView, AppViewModel>
    {
        public App(ILifetimeScope scope)
        {
            Scope = scope;
        }
    }
}
