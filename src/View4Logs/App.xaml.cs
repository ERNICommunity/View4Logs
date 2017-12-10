using System;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using Autofac.Features.ResolveAnything;
using View4Logs.Common.Interfaces;
using View4Logs.Core.Filters;
using View4Logs.Core.Services;
using View4Logs.Theme;
using View4Logs.Theme.Brushes;
using View4Logs.UI.Control;
using View4Logs.UI.Interfaces;
using View4Logs.UI.Services;
using View4Logs.UI.Theme;

namespace View4Logs
{
    public sealed partial class App : Application
    {
        static App()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(((Exception)e.ExceptionObject).Message);
        }

        public App()
        {
            Exit += OnExit;
        }

        public IContainer Container { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container = ContainerFactory();

            var window = Container.Resolve<AppWindow>();
            window.Show();
        }

        private IContainer ContainerFactory()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            // Dispatcher
            builder.RegisterInstance(Dispatcher);

            builder.RegisterType<LogFilterResultsService>().As<ILogFilterResultsService>().SingleInstance();
            builder.RegisterType<LogFilterService>().As<ILogFilterService>().SingleInstance();
            builder.RegisterType<LogSourceService>().As<ILogSourceService>().SingleInstance();
            builder.RegisterType<LogSourceLevelFilter>().As<ILogSourceLevelFilter>().SingleInstance();
            builder.RegisterType<Log4JXmlLogFileImportService>().As<ILogFileImporter>().SingleInstance();
            //builder.RegisterType<Log4NetXmlLogFileImportService>().As<ILogFileImporter>().SingleInstance();
            //builder.RegisterType<JsonLogFileImportService>().As<ILogFileImporter>().SingleInstance();

            // Theme
            builder.RegisterType<BrushDarkTheme>().As<ThemeResourceDictionary>().SingleInstance().OnActivating(e => e.Instance.InitializeComponent());
            builder.RegisterType<BrushLightTheme>().As<ThemeResourceDictionary>().SingleInstance().OnActivating(e => e.Instance.InitializeComponent());

            builder.RegisterType<ThemeConfigurationService>()
                .As<IThemeConfigurationService>()
                .SingleInstance()
                .AutoActivate()
                .OnActivated(e => e.Instance.LoadConfiguration());

            return builder.Build();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            Container.Dispose();
        }
    }
}
