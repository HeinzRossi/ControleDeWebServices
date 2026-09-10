using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Vinculos;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ListaVinculosSistemaViewModel : ViewModelBase
    {
        private readonly IVinculoClienteSistemaService vinculoService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;
        private readonly Func<VinculoClienteSistemaViewModel> editorFactory;

        [ObservableProperty]
        private ObservableCollection<ClienteVinculoListItem> clientes = new ObservableCollection<ClienteVinculoListItem>();

        [ObservableProperty]
        private ClienteVinculoListItem selectedCliente;

        [ObservableProperty]
        private ObservableCollection<ClienteSistemaResumo> sistemas = new ObservableCollection<ClienteSistemaResumo>();

        [ObservableProperty]
        private bool isEditing;

        [ObservableProperty]
        private VinculoClienteSistemaViewModel editor;

        public ListaVinculosSistemaViewModel(
            IVinculoClienteSistemaService vinculoService,
            IToastService toastService,
            IConfirmDialogService confirmDialogService,
            Func<VinculoClienteSistemaViewModel> editorFactory)
        {
            this.vinculoService = vinculoService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
            this.editorFactory = editorFactory;
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
        }

        [RelayCommand]
        public void Editar()
        {
            if (SelectedCliente == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um cliente antes de editar os vinculos.", "Atencao"));
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
            IsEditing = true;
        }

        [RelayCommand]
        public async Task ExcluirAsync()
        {
            if (SelectedCliente == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um cliente antes de remover vinculos.", "Atencao"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Remover vinculos do cliente?",
                $"Todos os sistemas vinculados ao cliente \"{SelectedCliente.NomeCliente}\" serao removidos.",
                "Remover",
                "Cancelar",
                true));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                vinculoService.ExcluirVinculosDoCliente(SelectedCliente.IdCliente);
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

        partial void OnClientesChanged(ObservableCollection<ClienteVinculoListItem> value)
        {
            OnPropertyChanged(nameof(TotalRegistros));
            OnPropertyChanged(nameof(IsEmpty));
        }

        partial void OnSelectedClienteChanged(ClienteVinculoListItem value)
        {
            Sistemas = value == null
                ? new ObservableCollection<ClienteSistemaResumo>()
                : new ObservableCollection<ClienteSistemaResumo>(vinculoService.ListarSistemasDoCliente(value.IdCliente));
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
