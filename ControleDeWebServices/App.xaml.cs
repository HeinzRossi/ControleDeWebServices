using System;
using System.Windows;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Clientes;
using ControleDeWebServices.Application.Navigation;
using ControleDeWebServices.Application.Vinculos;
using ControleDeWebServices.Infrastructure.Cadastros;
using ControleDeWebServices.Infrastructure.Clientes;
using ControleDeWebServices.Infrastructure.Data;
using ControleDeWebServices.Infrastructure.Vinculos;
using ControleDeWebServices.Presentation.Feedback;
using ControleDeWebServices.Presentation.Navigation;
using ControleDeWebServices.ViewModels;
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
            services.AddTransient<IClientesService, ClientesService>();
            services.AddTransient<ISistemasService, SistemasService>();
            services.AddTransient<IServicosService, ServicosService>();
            services.AddTransient<ISecoesService, SecoesService>();
            services.AddTransient<IVinculoClienteSistemaService, VinculoClienteSistemaService>();
            services.AddTransient<IVinculoClienteServicoService, VinculoClienteServicoService>();

            services.AddSingleton<ToastService>();
            services.AddSingleton<IToastService>(provider => provider.GetRequiredService<ToastService>());

            services.AddSingleton<ConfirmDialogService>();
            services.AddSingleton<IConfirmDialogService>(provider => provider.GetRequiredService<ConfirmDialogService>());

            services.AddSingleton<WpfNavigationService>();
            services.AddSingleton<INavigationService>(provider => provider.GetRequiredService<WpfNavigationService>());

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<ClientesListViewModel>();
            services.AddTransient<SistemasListViewModel>();
            services.AddTransient<ServicosListViewModel>();
            services.AddTransient<SecoesListViewModel>();
            services.AddTransient<VinculoClienteSistemaViewModel>();
            services.AddTransient<VinculoClienteServicoViewModel>();
            services.AddTransient<Func<VinculoClienteSistemaViewModel>>(provider => () => provider.GetRequiredService<VinculoClienteSistemaViewModel>());
            services.AddTransient<Func<VinculoClienteServicoViewModel>>(provider => () => provider.GetRequiredService<VinculoClienteServicoViewModel>());
            services.AddTransient<ListaVinculosSistemaViewModel>();
            services.AddTransient<ListaVinculosServicoViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
