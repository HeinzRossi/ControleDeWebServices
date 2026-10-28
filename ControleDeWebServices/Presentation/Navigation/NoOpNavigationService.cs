using ControleDeWebServices.Application.Navigation;
using System;
using System.Threading.Tasks;

namespace ControleDeWebServices.Presentation.Navigation
{
    public sealed class NoOpNavigationService : INavigationService
    {
        public event EventHandler<NavigationChangedEventArgs> Navigated;

        public NavigationRoute CurrentRoute { get; private set; } = NavigationRoute.WebServices;

        public Task NavigateAsync(NavigationRoute route)
        {
            CurrentRoute = route;
            Navigated?.Invoke(this, new NavigationChangedEventArgs(route));
            return Task.CompletedTask;
        }
    }
}
