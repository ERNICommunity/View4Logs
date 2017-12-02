using System;
using System.Windows;
using Autofac;
using Autofac.Features.ResolveAnything;
using View4Logs.Common.Interfaces;
using View4Logs.Services;
using View4Logs.UI.Control;

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
            System.Diagnostics.Debug.WriteLine(((Exception) e.ExceptionObject).Message);
        }

        public App()
        {
            Container = ContainerFactory();
            Exit += OnExit;
        }

        public IContainer Container { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = Container.Resolve<AppWindow>();
            window.Show();
        }

        private IContainer ContainerFactory()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            builder.RegisterType<LogFilterResultsService>().As<ILogFilterResultsService>().SingleInstance();
            builder.RegisterType<LogFilterService>().As<ILogFilterService>().SingleInstance();
            builder.RegisterType<LogSourceService>().As<ILogSourceService>().SingleInstance();
            //builder.RegisterType<Log4JXmlLogFileImportService>().As<ILogFileImporter>().SingleInstance();
            //builder.RegisterType<Log4NetXmlLogFileImportService>().As<ILogFileImporter>().SingleInstance();
            builder.RegisterType<JsonLogFileImportService>().As<ILogFileImporter>().SingleInstance();

            return builder.Build();
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            Container.Dispose();
        }
    }
}
