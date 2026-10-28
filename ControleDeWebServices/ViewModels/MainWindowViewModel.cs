using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Navigation;
using System;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        [ObservableProperty]
        private NavigationRoute activeRoute = NavigationRoute.WebServices;

        public MainWindowViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.navigationService.Navigated += NavigationService_Navigated;

            NavigateWebServicesCommand = new AsyncRelayCommand(() => NavigateAsync(NavigationRoute.WebServices));
            NavigateClientesCommand = new AsyncRelayCommand(() => NavigateAsync(NavigationRoute.Clientes));
            NavigateSistemasCommand = new AsyncRelayCommand(() => NavigateAsync(NavigationRoute.Sistemas));
            NavigateServicosCommand = new AsyncRelayCommand(() => NavigateAsync(NavigationRoute.Servicos));
            NavigateSecoesCommand = new AsyncRelayCommand(() => NavigateAsync(NavigationRoute.Secoes));
            NavigateVinculoClienteSistemaCommand = new AsyncRelayCommand(() => NavigateAsync(NavigationRoute.VinculoClienteSistema));
            NavigateVinculoClienteServicoCommand = new AsyncRelayCommand(() => NavigateAsync(NavigationRoute.VinculoClienteServico));
            ExitCommand = new RelayCommand(() => ExitRequested?.Invoke(this, EventArgs.Empty));
        }

        public IAsyncRelayCommand NavigateWebServicesCommand { get; }
        public IAsyncRelayCommand NavigateClientesCommand { get; }
        public IAsyncRelayCommand NavigateSistemasCommand { get; }
        public IAsyncRelayCommand NavigateServicosCommand { get; }
        public IAsyncRelayCommand NavigateSecoesCommand { get; }
        public IAsyncRelayCommand NavigateVinculoClienteSistemaCommand { get; }
        public IAsyncRelayCommand NavigateVinculoClienteServicoCommand { get; }
        public IRelayCommand ExitCommand { get; }

        public event EventHandler ExitRequested;

        private async Task NavigateAsync(NavigationRoute route)
        {
            await navigationService.NavigateAsync(route);
        }

        private void NavigationService_Navigated(object sender, NavigationChangedEventArgs e)
        {
            ActiveRoute = e.Route;
        }
    }
}
