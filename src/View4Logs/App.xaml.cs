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
        private DemoLogSource _demoSource;

        public App()
        {
            Container = ContainerFactory();
        }

        public IContainer Container { get; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = Container.Resolve<AppWindow>();
            window.Show();

            _demoSource = Container.Resolve<DemoLogSource>();
            _demoSource.Start();
        }

        private IContainer ContainerFactory()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            builder.RegisterType<LogFilterResultsService>().As<ILogFilterResultsService>().SingleInstance();
            builder.RegisterType<LogFilterService>().As<ILogFilterService>().SingleInstance();
            builder.RegisterType<LogSourceService>().As<ILogSourceService>().SingleInstance();

            return builder.Build();
        }
    }
}
