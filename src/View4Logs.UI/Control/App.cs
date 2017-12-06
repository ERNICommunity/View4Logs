using Autofac;
using View4Logs.UI.Base;
using View4Logs.UI.Interfaces;
using View4Logs.UI.Services;
using View4Logs.UI.View;
using View4Logs.UI.ViewModel;

namespace View4Logs.UI.Control
{
    public sealed class App : Component<AppView, AppViewModel>
    {
        public App(ILifetimeScope scope)
        {
            var childScope = scope.BeginLifetimeScope(builder =>
            {
                builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            });

            SetCurrentValue(ScopeProperty, childScope);
        }
    }
}
