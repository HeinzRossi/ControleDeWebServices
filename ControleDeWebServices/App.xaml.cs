using System;
using System.Windows;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Navigation;
using ControleDeWebServices.Infrastructure.Data;
using ControleDeWebServices.Presentation.Feedback;
using ControleDeWebServices.Presentation.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace ControleDeWebServices
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Services = ConfigureServices();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (Services is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDadosDbContextFactory, DadosDbContextFactory>();
            services.AddSingleton<IToastService, NoOpToastService>();
            services.AddSingleton<IConfirmDialogService, NoOpConfirmDialogService>();
            services.AddSingleton<INavigationService, NoOpNavigationService>();

            return services.BuildServiceProvider();
        }
    }
}
