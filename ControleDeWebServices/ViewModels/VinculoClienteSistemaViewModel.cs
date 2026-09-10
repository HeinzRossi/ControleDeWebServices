using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Vinculos;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class VinculoClienteSistemaViewModel : ViewModelBase
    {
        private readonly IVinculoClienteSistemaService vinculoService;
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
        private ObservableCollection<VinculoItem> sistemasDisponiveis = new ObservableCollection<VinculoItem>();

        [ObservableProperty]
        private ObservableCollection<VinculoItem> sistemasVinculados = new ObservableCollection<VinculoItem>();

        [ObservableProperty]
        private VinculoItem selectedSistemaDisponivel;

        [ObservableProperty]
        private VinculoItem selectedSistemaVinculado;

        [ObservableProperty]
        private bool hasPendingChanges;

        public VinculoClienteSistemaViewModel(IVinculoClienteSistemaService vinculoService, IToastService toastService)
        {
            this.vinculoService = vinculoService;
            this.toastService = toastService;
        }

        public string PendingText => HasPendingChanges ? "Alteracoes pendentes" : "Sem alteracoes pendentes";
        public bool HasCliente => SelectedCliente != null;
        public bool HasSistemas => SistemasDisponiveis.Count > 0 || SistemasVinculados.Count > 0;

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

            Clientes = new ObservableCollection<VinculoOptionItem>(vinculoService.ListarClientesPorUf(SelectedUf, false));
            SelectedCliente = null;
            LimparSistemas();
        }

        [RelayCommand]
        public void SelecionarCliente()
        {
            if (SelectedCliente == null)
            {
                LimparSistemas();
                return;
            }

            SistemasDisponiveis = new ObservableCollection<VinculoItem>(vinculoService.ListarSistemasDisponiveis(SelectedCliente.Uf, SelectedCliente.Id));
            SistemasVinculados = new ObservableCollection<VinculoItem>(vinculoService.ListarSistemasVinculados(SelectedCliente.Id));
            HasPendingChanges = false;
            OnPropertyChanged(nameof(HasSistemas));
        }

        [RelayCommand]
        public void Adicionar(VinculoItem sistema)
        {
            var item = sistema ?? SelectedSistemaDisponivel;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um sistema disponivel antes de adicionar.", "Atencao"));
                return;
            }

            Mover(item, SistemasDisponiveis, SistemasVinculados);
        }

        [RelayCommand]
        public void Remover(VinculoItem sistema)
        {
            var item = sistema ?? SelectedSistemaVinculado;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um sistema vinculado antes de remover.", "Atencao"));
                return;
            }

            Mover(item, SistemasVinculados, SistemasDisponiveis);
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

            try
            {
                vinculoService.Salvar(SelectedCliente.Id, SistemasVinculados.Select(item => item.Id));
                HasPendingChanges = false;
                toastService.Show(new ToastRequest(ToastKind.Success, "Vinculos salvos com sucesso.", "Sucesso"));
                SelecionarCliente();
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
            SelecionarCliente();
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

        partial void OnHasPendingChangesChanged(bool value)
        {
            OnPropertyChanged(nameof(PendingText));
        }

        partial void OnSistemasDisponiveisChanged(ObservableCollection<VinculoItem> value)
        {
            OnPropertyChanged(nameof(HasSistemas));
        }

        partial void OnSistemasVinculadosChanged(ObservableCollection<VinculoItem> value)
        {
            OnPropertyChanged(nameof(HasSistemas));
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
            OnPropertyChanged(nameof(HasSistemas));
        }

        private void LimparSistemas()
        {
            SistemasDisponiveis = new ObservableCollection<VinculoItem>();
            SistemasVinculados = new ObservableCollection<VinculoItem>();
            HasPendingChanges = false;
        }
    }
}
