using ControleDeWebServices.Application.Navigation;
using ControleDeWebServices.View;
using ControleDeWebServices.View.Cadastro;
using ControleDeWebServices.View.Vinculos;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ControleDeWebServices.Presentation.Navigation
{
    public sealed class WpfNavigationService : INavigationService
    {
        private Frame frame;

        public event EventHandler<NavigationChangedEventArgs> Navigated;

        public NavigationRoute CurrentRoute { get; private set; } = NavigationRoute.WebServices;

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

        private static object CreatePage(NavigationRoute route)
        {
            switch (route)
            {
                case NavigationRoute.WebServices:
                    return new WebServices();
                case NavigationRoute.Clientes:
                    return new ListaDeClientes();
                case NavigationRoute.Sistemas:
                    return new ListaDeSistemas();
                case NavigationRoute.Servicos:
                    return new ListaDeServicos();
                case NavigationRoute.Secoes:
                    return new ListaDeSecoes();
                case NavigationRoute.VinculoClienteSistema:
                    return new ListaVinculosSistema();
                case NavigationRoute.VinculoClienteServico:
                    return new ListaVinculosServico();
                default:
                    throw new ArgumentOutOfRangeException(nameof(route), route, null);
            }
        }
    }
}
