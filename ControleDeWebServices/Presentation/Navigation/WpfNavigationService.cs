using ControleDeWebServices.Application.Navigation;
using ControleDeWebServices.View;
using ControleDeWebServices.View.Cadastro;
using ControleDeWebServices.View.Vinculos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ControleDeWebServices.Presentation.Navigation
{
    public sealed class WpfNavigationService : INavigationService
    {
        private readonly IServiceProvider serviceProvider;
        private Frame frame;

        public event EventHandler<NavigationChangedEventArgs> Navigated;

        public NavigationRoute CurrentRoute { get; private set; } = NavigationRoute.WebServices;

        public WpfNavigationService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Attach(Frame hostFrame)
        {
            frame = hostFrame ?? throw new ArgumentNullException(nameof(hostFrame));
        }

        public Task NavigateAsync(NavigationRoute route)
        {
            if (frame == null)
            {
                throw new InvalidOperationException("O host de navegacao ainda nao foi configurado.");
            }

            frame.Content = CreatePage(route);
            CurrentRoute = route;
            Navigated?.Invoke(this, new NavigationChangedEventArgs(route));
            return Task.CompletedTask;
        }

        private object CreatePage(NavigationRoute route)
        {
            switch (route)
            {
                case NavigationRoute.WebServices:
                    return ActivatorUtilities.CreateInstance<WebServices>(serviceProvider);
                case NavigationRoute.Clientes:
                    return ActivatorUtilities.CreateInstance<ListaDeClientes>(serviceProvider);
                case NavigationRoute.Sistemas:
                    return ActivatorUtilities.CreateInstance<ListaDeSistemas>(serviceProvider);
                case NavigationRoute.Servicos:
                    return ActivatorUtilities.CreateInstance<ListaDeServicos>(serviceProvider);
                case NavigationRoute.Secoes:
                    return ActivatorUtilities.CreateInstance<ListaDeSecoes>(serviceProvider);
                case NavigationRoute.VinculoClienteSistema:
                    return ActivatorUtilities.CreateInstance<ListaVinculosSistema>(serviceProvider);
                case NavigationRoute.VinculoClienteServico:
                    return ActivatorUtilities.CreateInstance<ListaVinculosServico>(serviceProvider);
                default:
                    throw new ArgumentOutOfRangeException(nameof(route), route, null);
            }
        }
    }
}
