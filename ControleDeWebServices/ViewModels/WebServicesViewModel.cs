using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Operacao;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class WebServicesViewModel : ViewModelBase
    {
        private readonly IWebServicesService webServicesService;
        private readonly IWebServiceExecutionService executionService;
        private readonly IToastService toastService;

        [ObservableProperty]
        private ObservableCollection<WebServiceItem> maisAcessados = new ObservableCollection<WebServiceItem>();

        [ObservableProperty]
        private WebServiceItem selectedMaisAcessado;

        [ObservableProperty]
        private ObservableCollection<string> ufs = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedUf;

        [ObservableProperty]
        private ObservableCollection<WebServiceClienteOption> clientes = new ObservableCollection<WebServiceClienteOption>();

        [ObservableProperty]
        private WebServiceClienteOption selectedCliente;

        [ObservableProperty]
        private string codigoCliente;

        [ObservableProperty]
        private ObservableCollection<WebServiceItem> sistemas = new ObservableCollection<WebServiceItem>();

        [ObservableProperty]
        private WebServiceItem selectedSistema;

        [ObservableProperty]
        private string executionStatus = "Pronto para operação.";

        [ObservableProperty]
        private ObservableCollection<string> executionSteps = new ObservableCollection<string>();

        public WebServicesViewModel(
            IWebServicesService webServicesService,
            IWebServiceExecutionService executionService,
            IToastService toastService)
        {
            this.webServicesService = webServicesService;
            this.executionService = executionService;
            this.toastService = toastService;
        }

        public bool HasSistemas => Sistemas.Count > 0;
        public bool HasMaisAcessados => MaisAcessados.Count > 0;

        [RelayCommand]
        public void Carregar()
        {
            try
            {
                MaisAcessados = new ObservableCollection<WebServiceItem>(webServicesService.ListarMaisAcessados());
                Ufs = new ObservableCollection<string>(webServicesService.ListarUfs());
                ExecutionStatus = "WebServices carregados.";
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível carregar WebServices. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void BuscarPorCodigo()
        {
            if (string.IsNullOrWhiteSpace(CodigoCliente))
            {
                Sistemas = new ObservableCollection<WebServiceItem>();
                return;
            }

            if (!int.TryParse(CodigoCliente, out var codigo))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe um código de cliente válido.", "Atenção"));
                return;
            }

            try
            {
                Sistemas = new ObservableCollection<WebServiceItem>(webServicesService.ListarSistemasPorCodigoCliente(codigo));
                SelectedSistema = Sistemas.Count > 0 ? Sistemas[0] : null;
                ExecutionStatus = Sistemas.Count == 0 ? "Nenhum WebService encontrado para o código informado." : $"{Sistemas.Count} WebServices encontrados.";
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível buscar por código. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public async Task ExecutarSelecionadoAsync(WebServiceItem item)
        {
            var selected = item ?? SelectedSistema ?? SelectedMaisAcessado;
            if (selected == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um WebService antes de executar.", "Atenção"));
                return;
            }

            IsBusy = true;
            ExecutionSteps = new ObservableCollection<string>();
            ExecutionStatus = $"Executando {selected.NomeSistema}...";

            try
            {
                var result = await executionService.ExecutarAsync(selected.IdClientesSistema, step =>
                {
                    AddExecutionStep(step.Name);
                });

                if (result.Success)
                {
                    toastService.Show(new ToastRequest(ToastKind.Success, result.Message, "Sucesso"));
                    Carregar();
                }
                else
                {
                    toastService.Show(new ToastRequest(ToastKind.Error, result.Message, "Erro"));
                }

                ExecutionStatus = result.Message;
            }
            catch (Exception ex)
            {
                ExecutionStatus = "Falha na execução do WebService.";
                toastService.Show(new ToastRequest(ToastKind.Error, $"Falha na execução do WebService. {ex.Message}", "Erro"));
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public void AtualizarUrl(WebServiceItem item)
        {
            var selected = item ?? SelectedSistema ?? SelectedMaisAcessado;
            if (selected == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um WebService antes de atualizar a URL.", "Atenção"));
                return;
            }

            try
            {
                webServicesService.AtualizarUrl(selected.IdClientesSistema);
                toastService.Show(new ToastRequest(ToastKind.Success, "URL atualizada com sucesso.", "Sucesso"));
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível atualizar a URL. {ex.Message}", "Erro"));
            }
        }

        partial void OnMaisAcessadosChanged(ObservableCollection<WebServiceItem> value)
        {
            OnPropertyChanged(nameof(HasMaisAcessados));
        }

        partial void OnSistemasChanged(ObservableCollection<WebServiceItem> value)
        {
            OnPropertyChanged(nameof(HasSistemas));
        }

        partial void OnCodigoClienteChanged(string value)
        {
            BuscarPorCodigo();
        }

        partial void OnSelectedUfChanged(string value)
        {
            Clientes = string.IsNullOrWhiteSpace(value)
                ? new ObservableCollection<WebServiceClienteOption>()
                : new ObservableCollection<WebServiceClienteOption>(webServicesService.ListarClientesPorUf(value));
            SelectedCliente = null;
            Sistemas = new ObservableCollection<WebServiceItem>();
        }

        partial void OnSelectedClienteChanged(WebServiceClienteOption value)
        {
            Sistemas = value == null
                ? new ObservableCollection<WebServiceItem>()
                : new ObservableCollection<WebServiceItem>(webServicesService.ListarSistemasPorCliente(value.IdCliente));
            SelectedSistema = Sistemas.Count > 0 ? Sistemas[0] : null;
        }

        private void AddExecutionStep(string stepName)
        {
            void Apply()
            {
                ExecutionStatus = stepName;
                ExecutionSteps.Add(stepName);
            }

            var dispatcher = System.Windows.Application.Current?.Dispatcher;
            if (dispatcher != null && !dispatcher.CheckAccess())
            {
                dispatcher.Invoke((Action)Apply);
                return;
            }

            Apply();
        }
    }
}
