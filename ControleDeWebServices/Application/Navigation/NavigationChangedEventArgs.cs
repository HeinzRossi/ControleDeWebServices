using System;

namespace ControleDeWebServices.Application.Navigation
{
    public sealed class NavigationChangedEventArgs : EventArgs
    {
        public NavigationChangedEventArgs(NavigationRoute route)
        {
            Route = route;
        }

        public NavigationRoute Route { get; }
    }
}
