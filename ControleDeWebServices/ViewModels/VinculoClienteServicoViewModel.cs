using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Vinculos;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class VinculoClienteServicoViewModel : ViewModelBase
    {
        private readonly IVinculoClienteServicoService vinculoService;
        private readonly IToastService toastService;

        [ObservableProperty]
        private ObservableCollection<string> ufs = new ObservableCollection<string>();

        [ObservableProperty]
        private string selectedUf;

        [ObservableProperty]
        private ObservableCollection<VinculoOptionItem> clientes = new ObservableCollection<VinculoOptionItem>();

        [ObservableProperty]
        private VinculoOptionItem selectedCliente;

        [ObservableProperty]
        private ObservableCollection<VinculoOptionItem> sistemas = new ObservableCollection<VinculoOptionItem>();

        [ObservableProperty]
        private VinculoOptionItem selectedSistema;

        [ObservableProperty]
        private ObservableCollection<VinculoItem> servicosDisponiveis = new ObservableCollection<VinculoItem>();

        [ObservableProperty]
        private ObservableCollection<VinculoItem> servicosVinculados = new ObservableCollection<VinculoItem>();

        [ObservableProperty]
        private VinculoItem selectedServicoDisponivel;

        [ObservableProperty]
        private VinculoItem selectedServicoVinculado;

        [ObservableProperty]
        private bool hasPendingChanges;

        public VinculoClienteServicoViewModel(IVinculoClienteServicoService vinculoService, IToastService toastService)
        {
            this.vinculoService = vinculoService;
            this.toastService = toastService;
        }

        public string PendingText => HasPendingChanges ? "Alteracoes pendentes" : "Sem alteracoes pendentes";
        public bool HasCliente => SelectedCliente != null;
        public bool HasSistema => SelectedSistema != null;
        public bool HasServicos => ServicosDisponiveis.Count > 0 || ServicosVinculados.Count > 0;

        [RelayCommand]
        public void Carregar()
        {
            Ufs = new ObservableCollection<string>(vinculoService.ListarUfs());
        }

        [RelayCommand]
        public void SelecionarUf()
        {
            if (string.IsNullOrWhiteSpace(SelectedUf))
            {
                return;
            }

            Clientes = new ObservableCollection<VinculoOptionItem>(vinculoService.ListarClientesPorUf(SelectedUf));
            SelectedCliente = null;
            LimparSistemasEServicos();
        }

        [RelayCommand]
        public void SelecionarCliente()
        {
            if (SelectedCliente == null)
            {
                LimparSistemasEServicos();
                return;
            }

            Sistemas = new ObservableCollection<VinculoOptionItem>(vinculoService.ListarSistemasDoCliente(SelectedCliente.Id));
            SelectedSistema = null;
            LimparServicos();
        }

        [RelayCommand]
        public void SelecionarSistema()
        {
            if (SelectedCliente == null || SelectedSistema == null)
            {
                LimparServicos();
                return;
            }

            ServicosDisponiveis = new ObservableCollection<VinculoItem>(vinculoService.ListarServicosDisponiveis(SelectedCliente.Uf, SelectedCliente.Id, SelectedSistema.Id));
            ServicosVinculados = new ObservableCollection<VinculoItem>(vinculoService.ListarServicosVinculados(SelectedCliente.Id, SelectedSistema.Id));
            HasPendingChanges = false;
            OnPropertyChanged(nameof(HasServicos));
        }

        [RelayCommand]
        public void Adicionar(VinculoItem servico)
        {
            var item = servico ?? SelectedServicoDisponivel;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um servico disponivel antes de adicionar.", "Atencao"));
                return;
            }

            Mover(item, ServicosDisponiveis, ServicosVinculados);
        }

        [RelayCommand]
        public void Remover(VinculoItem servico)
        {
            var item = servico ?? SelectedServicoVinculado;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um servico vinculado antes de remover.", "Atencao"));
                return;
            }

            Mover(item, ServicosVinculados, ServicosDisponiveis);
        }

        [RelayCommand]
        public void Salvar()
        {
            TrySalvar();
        }

        public bool TrySalvar()
        {
            if (SelectedCliente == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um cliente antes de salvar.", "Atencao"));
                return false;
            }

            if (SelectedSistema == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um sistema antes de salvar.", "Atencao"));
                return false;
            }

            try
            {
                vinculoService.Salvar(SelectedCliente.Id, SelectedSistema.Id, ServicosVinculados.Select(item => item.Id));
                HasPendingChanges = false;
                toastService.Show(new ToastRequest(ToastKind.Success, "Vinculos salvos com sucesso.", "Sucesso"));
                SelecionarSistema();
                return true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel salvar os vinculos. {ex.Message}", "Erro"));
                return false;
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            SelecionarSistema();
        }

        partial void OnSelectedUfChanged(string value)
        {
            SelecionarUf();
        }

        partial void OnSelectedClienteChanged(VinculoOptionItem value)
        {
            OnPropertyChanged(nameof(HasCliente));
            SelecionarCliente();
        }

        partial void OnSelectedSistemaChanged(VinculoOptionItem value)
        {
            OnPropertyChanged(nameof(HasSistema));
            SelecionarSistema();
        }

        partial void OnHasPendingChangesChanged(bool value)
        {
            OnPropertyChanged(nameof(PendingText));
        }

        partial void OnServicosDisponiveisChanged(ObservableCollection<VinculoItem> value)
        {
            OnPropertyChanged(nameof(HasServicos));
        }

        partial void OnServicosVinculadosChanged(ObservableCollection<VinculoItem> value)
        {
            OnPropertyChanged(nameof(HasServicos));
        }

        private void Mover(VinculoItem item, ObservableCollection<VinculoItem> origem, ObservableCollection<VinculoItem> destino)
        {
            if (!destino.Any(destinoItem => destinoItem.Id == item.Id))
            {
                item.IsPending = true;
                destino.Add(item);
            }

            origem.Remove(item);
            HasPendingChanges = true;
            OnPropertyChanged(nameof(HasServicos));
        }

        private void LimparSistemasEServicos()
        {
            Sistemas = new ObservableCollection<VinculoOptionItem>();
            SelectedSistema = null;
            LimparServicos();
        }

        private void LimparServicos()
        {
            ServicosDisponiveis = new ObservableCollection<VinculoItem>();
            ServicosVinculados = new ObservableCollection<VinculoItem>();
            HasPendingChanges = false;
        }
    }
}
