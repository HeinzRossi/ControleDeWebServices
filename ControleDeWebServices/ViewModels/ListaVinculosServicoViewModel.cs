using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Vinculos;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ListaVinculosServicoViewModel : ViewModelBase
    {
        private readonly IVinculoClienteServicoService vinculoService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;
        private readonly Func<VinculoClienteServicoViewModel> editorFactory;
        private readonly Func<ConfiguracaoServicoViewModel> configuracaoFactory;

        [ObservableProperty]
        private ObservableCollection<ClienteVinculoListItem> clientes = new ObservableCollection<ClienteVinculoListItem>();

        [ObservableProperty]
        private ClienteVinculoListItem selectedCliente;

        [ObservableProperty]
        private ObservableCollection<ClienteServicoSistemaResumo> sistemas = new ObservableCollection<ClienteServicoSistemaResumo>();

        [ObservableProperty]
        private ClienteServicoSistemaResumo selectedSistema;

        [ObservableProperty]
        private ObservableCollection<ClienteServicoResumo> servicos = new ObservableCollection<ClienteServicoResumo>();

        [ObservableProperty]
        private ClienteServicoResumo selectedServico;

        [ObservableProperty]
        private bool isEditing;

        [ObservableProperty]
        private VinculoClienteServicoViewModel editor;

        [ObservableProperty]
        private bool isConfiguring;

        [ObservableProperty]
        private ConfiguracaoServicoViewModel configuracao;

        public ListaVinculosServicoViewModel(
            IVinculoClienteServicoService vinculoService,
            IToastService toastService,
            IConfirmDialogService confirmDialogService,
            Func<VinculoClienteServicoViewModel> editorFactory,
            Func<ConfiguracaoServicoViewModel> configuracaoFactory)
        {
            this.vinculoService = vinculoService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
            this.editorFactory = editorFactory;
            this.configuracaoFactory = configuracaoFactory;
        }

        public string TotalRegistros => Clientes.Count == 1 ? "1 cliente vinculado" : $"{Clientes.Count} clientes vinculados";
        public bool IsEmpty => Clientes.Count == 0;

        [RelayCommand]
        public void Carregar()
        {
            try
            {
                var selecionado = SelectedCliente?.IdCliente;
                Clientes = new ObservableCollection<ClienteVinculoListItem>(vinculoService.ListarClientesComVinculos());
                SelectedCliente = selecionado.HasValue ? FindCliente(selecionado.Value) : null;
                if (SelectedCliente == null && Clientes.Count > 0)
                {
                    SelectedCliente = Clientes[0];
                }
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel carregar vinculos. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Incluir()
        {
            Editor = editorFactory();
            Editor.CarregarCommand.Execute(null);
            IsEditing = true;
            IsConfiguring = false;
        }

        [RelayCommand]
        public void Editar()
        {
            if (SelectedCliente == null || SelectedSistema == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um cliente e um sistema antes de editar.", "Atencao"));
                return;
            }

            Editor = editorFactory();
            Editor.CarregarCommand.Execute(null);
            Editor.SelectedUf = SelectedCliente.Uf;
            Editor.SelectedCliente = new VinculoOptionItem
            {
                Id = SelectedCliente.IdCliente,
                Nome = SelectedCliente.NomeCliente,
                Uf = SelectedCliente.Uf
            };
            Editor.SelectedSistema = new VinculoOptionItem
            {
                Id = SelectedSistema.IdSistemas,
                Nome = SelectedSistema.NomeSistema,
                Uf = SelectedSistema.Uf
            };
            IsEditing = true;
            IsConfiguring = false;
        }

        [RelayCommand]
        public void Configurar()
        {
            if (SelectedServico == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um serviço antes de configurar.", "Atencao"));
                return;
            }

            Configuracao = configuracaoFactory();
            Configuracao.RequestClose += (sender, args) => CancelarConfiguracao();
            Configuracao.Carregar(SelectedServico.IdClienteServico);
            IsConfiguring = true;
            IsEditing = false;
        }

        [RelayCommand]
        public async Task ExcluirAsync()
        {
            if (SelectedCliente == null || SelectedSistema == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um cliente e um sistema antes de remover vinculos.", "Atencao"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Remover vinculos do sistema?",
                $"Todos os servicos vinculados ao sistema \"{SelectedSistema.NomeSistema}\" serao removidos.",
                "Remover",
                "Cancelar",
                true));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                vinculoService.ExcluirVinculosDoSistema(SelectedSistema.IdCliente, SelectedSistema.IdSistemas);
                toastService.Show(new ToastRequest(ToastKind.Success, "Vinculos removidos com sucesso.", "Sucesso"));
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel remover os vinculos. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void ConcluirEdicao()
        {
            if (Editor != null && Editor.TrySalvar())
            {
                IsEditing = false;
                Carregar();
            }
        }

        [RelayCommand]
        public void CancelarEdicao()
        {
            IsEditing = false;
        }

        [RelayCommand]
        public void ConcluirConfiguracao()
        {
            if (Configuracao != null && Configuracao.TrySalvar())
            {
                CancelarConfiguracao();
                Carregar();
            }
        }

        [RelayCommand]
        public void CancelarConfiguracao()
        {
            IsConfiguring = false;
            Configuracao = null;
        }

        partial void OnClientesChanged(ObservableCollection<ClienteVinculoListItem> value)
        {
            OnPropertyChanged(nameof(TotalRegistros));
            OnPropertyChanged(nameof(IsEmpty));
        }

        partial void OnSelectedClienteChanged(ClienteVinculoListItem value)
        {
            Sistemas = value == null
                ? new ObservableCollection<ClienteServicoSistemaResumo>()
                : new ObservableCollection<ClienteServicoSistemaResumo>(vinculoService.ListarSistemasComServicosDoCliente(value.IdCliente));
            SelectedSistema = Sistemas.Count > 0 ? Sistemas[0] : null;
        }

        partial void OnSelectedSistemaChanged(ClienteServicoSistemaResumo value)
        {
            Servicos = value == null
                ? new ObservableCollection<ClienteServicoResumo>()
                : new ObservableCollection<ClienteServicoResumo>(vinculoService.ListarServicosDoSistema(value.IdCliente, value.IdSistemas));
            SelectedServico = Servicos.Count > 0 ? Servicos[0] : null;
        }

        private ClienteVinculoListItem FindCliente(int idCliente)
        {
            foreach (var cliente in Clientes)
            {
                if (cliente.IdCliente == idCliente)
                {
                    return cliente;
                }
            }

            return null;
        }
    }
}
