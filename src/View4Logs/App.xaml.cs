using System.Windows;
using Autofac;
using Autofac.Features.ResolveAnything;
using View4Logs.Services;
using View4Logs.UI.Control;

namespace View4Logs
{
    public sealed partial class App : Application
    {
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
        }

        private IContainer ContainerFactory()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            builder.RegisterType<LogSourceService>()
                .As<ILogSourceService>();

            return builder.Build();
        }
    }
}
