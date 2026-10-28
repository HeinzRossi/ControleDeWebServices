using System;
using System.Threading.Tasks;

namespace ControleDeWebServices.Application.Navigation
{
    public interface INavigationService
    {
        event EventHandler<NavigationChangedEventArgs> Navigated;

        NavigationRoute CurrentRoute { get; }

        Task NavigateAsync(NavigationRoute route);
    }
}
