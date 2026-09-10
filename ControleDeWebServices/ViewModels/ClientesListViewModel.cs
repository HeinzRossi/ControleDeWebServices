using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Clientes;
using ControleDeWebServices.Application.Feedback;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ClientesListViewModel : ViewModelBase
    {
        private readonly IClientesService clientesService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;

        [ObservableProperty]
        private ObservableCollection<ClienteListItem> clientes = new ObservableCollection<ClienteListItem>();

        [ObservableProperty]
        private ClienteListItem selectedCliente;

        [ObservableProperty]
        private ClienteEditViewModel editor;

        [ObservableProperty]
        private bool isEditing;

        public ClientesListViewModel(
            IClientesService clientesService,
            IToastService toastService,
            IConfirmDialogService confirmDialogService)
        {
            this.clientesService = clientesService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
        }

        public bool IsListVisible => !IsEditing;

        public string TotalRegistros => Clientes.Count == 1 ? "1 registro" : $"{Clientes.Count} registros";

        public bool HasClientes => Clientes.Count > 0;

        public bool IsEmpty => Clientes.Count == 0;

        [RelayCommand]
        public void Carregar()
        {
            try
            {
                var selecionado = SelectedCliente?.IdCliente;
                Clientes = new ObservableCollection<ClienteListItem>(clientesService.Listar());
                SelectedCliente = selecionado.HasValue ? FindCliente(selecionado.Value) : null;
                if (SelectedCliente == null && Clientes.Count > 0)
                {
                    SelectedCliente = Clientes[0];
                }
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel carregar clientes. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Incluir()
        {
            Editor = ClienteEditViewModel.Novo();
            IsEditing = true;
        }

        [RelayCommand]
        public void Editar(ClienteListItem cliente)
        {
            var item = cliente ?? SelectedCliente;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um cliente antes de editar.", "Atenção"));
                return;
            }

            try
            {
                Editor = ClienteEditViewModel.FromEditor(clientesService.ObterParaEdicao(item.IdCliente));
                IsEditing = true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel abrir o cliente. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public async Task ExcluirAsync(ClienteListItem cliente)
        {
            var item = cliente ?? SelectedCliente;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um cliente antes de excluir.", "Atenção"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Excluir este cliente?",
                $"O cliente \"{item.NomeCliente}\" sera removido permanentemente.",
                "Excluir",
                "Cancelar",
                true));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                clientesService.Excluir(item.IdCliente);
                toastService.Show(new ToastRequest(ToastKind.Success, "Cliente excluido com sucesso.", "Sucesso"));
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel excluir o cliente. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Salvar()
        {
            if (Editor == null)
            {
                return;
            }

            if (!ValidarEditor())
            {
                return;
            }

            try
            {
                clientesService.Salvar(Editor.ToEditor());
                toastService.Show(new ToastRequest(ToastKind.Success, "Cliente salvo com sucesso.", "Sucesso"));
                IsEditing = false;
                Editor = null;
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel salvar o cliente. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            Editor = null;
            IsEditing = false;
        }

        partial void OnClientesChanged(ObservableCollection<ClienteListItem> value)
        {
            OnPropertyChanged(nameof(TotalRegistros));
            OnPropertyChanged(nameof(HasClientes));
            OnPropertyChanged(nameof(IsEmpty));
        }

        partial void OnIsEditingChanged(bool value)
        {
            OnPropertyChanged(nameof(IsListVisible));
        }

        private bool ValidarEditor()
        {
            if (string.IsNullOrWhiteSpace(Editor.NomeCliente))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe o nome do cliente antes de salvar.", "Atenção"));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Editor.Uf))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe a UF do cliente antes de salvar.", "Atenção"));
                return false;
            }

            return true;
        }

        private ClienteListItem FindCliente(int idCliente)
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
