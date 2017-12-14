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
                builder.RegisterType<LayoutService>().As<ILayoutService>().SingleInstance();
                builder.RegisterType<LogFileImportService>().As<ILogFileImportService>().SingleInstance(); ;
                builder.RegisterType<LogsViewService>().As<ILogsViewService>().SingleInstance();
                builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
                builder.RegisterType<TextSelectionProvider>().As<ITextSelectionProvider>().SingleInstance();
                builder.RegisterType<WebSearchService>().As<IWebSearchService>().SingleInstance();
            });

            SetCurrentValue(ScopeProperty, childScope);
        }
    }
}
