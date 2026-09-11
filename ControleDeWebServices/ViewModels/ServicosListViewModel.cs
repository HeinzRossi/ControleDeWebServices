using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Feedback;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ServicosListViewModel : ViewModelBase
    {
        private readonly IServicosService servicosService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;

        [ObservableProperty]
        private ObservableCollection<ServicoListItem> servicos = new ObservableCollection<ServicoListItem>();

        [ObservableProperty]
        private ServicoListItem selectedServico;

        [ObservableProperty]
        private ServicoEditViewModel editor;

        [ObservableProperty]
        private bool isEditing;

        public ServicosListViewModel(IServicosService servicosService, IToastService toastService, IConfirmDialogService confirmDialogService)
        {
            this.servicosService = servicosService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
        }

        public string TotalRegistros => Servicos.Count == 1 ? "1 registro" : $"{Servicos.Count} registros";
        public bool IsEmpty => Servicos.Count == 0;
        public bool CanUseListActions => !IsEditing;
        public bool IsListEnabled => CanUseListActions;

        [RelayCommand]
        public void Carregar()
        {
            try
            {
                var selecionado = SelectedServico?.IdServicos;
                Servicos = new ObservableCollection<ServicoListItem>(servicosService.Listar());
                SelectedServico = selecionado.HasValue ? FindServico(selecionado.Value) : null;
                if (SelectedServico == null && Servicos.Count > 0)
                {
                    SelectedServico = Servicos[0];
                }
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel carregar servicos. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public void Incluir()
        {
            if (IsEditing)
            {
                return;
            }

            Editor = ServicoEditViewModel.Novo();
            IsEditing = true;
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public void Editar(ServicoListItem servico)
        {
            if (IsEditing)
            {
                return;
            }

            var item = servico ?? SelectedServico;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um serviço antes de editar.", "Atenção"));
                return;
            }

            try
            {
                Editor = ServicoEditViewModel.FromEditor(servicosService.ObterParaEdicao(item.IdServicos));
                IsEditing = true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível abrir o serviço. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public async Task ExcluirAsync(ServicoListItem servico)
        {
            if (IsEditing)
            {
                return;
            }

            var item = servico ?? SelectedServico;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um serviço antes de excluir.", "Atenção"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Excluir este serviço?",
                $"O serviço \"{item.NomeServico}\" sera removido permanentemente.",
                "Excluir",
                "Cancelar",
                true));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                servicosService.Excluir(item.IdServicos);
                toastService.Show(new ToastRequest(ToastKind.Success, "Serviço excluido com sucesso.", "Sucesso"));
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível excluir o serviço. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Salvar()
        {
            if (Editor == null || !ValidarEditor())
            {
                return;
            }

            try
            {
                servicosService.Salvar(Editor.ToEditor());
                toastService.Show(new ToastRequest(ToastKind.Success, "Serviço salvo com sucesso.", "Sucesso"));
                Editor = null;
                IsEditing = false;
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível salvar o serviço. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            Editor = null;
            IsEditing = false;
        }

        partial void OnServicosChanged(ObservableCollection<ServicoListItem> value)
        {
            OnPropertyChanged(nameof(TotalRegistros));
            OnPropertyChanged(nameof(IsEmpty));
        }

        partial void OnIsEditingChanged(bool value)
        {
            OnPropertyChanged(nameof(CanUseListActions));
            OnPropertyChanged(nameof(IsListEnabled));
            IncluirCommand.NotifyCanExecuteChanged();
            EditarCommand.NotifyCanExecuteChanged();
            ExcluirCommand.NotifyCanExecuteChanged();
        }

        private bool ValidarEditor()
        {
            if (string.IsNullOrWhiteSpace(Editor.NomeServico))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe o nome do serviço antes de salvar.", "Atenção"));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Editor.Uf))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe a UF do serviço antes de salvar.", "Atenção"));
                return false;
            }

            return true;
        }

        private ServicoListItem FindServico(int idServicos)
        {
            foreach (var servico in Servicos)
            {
                if (servico.IdServicos == idServicos)
                {
                    return servico;
                }
            }

            return null;
        }
    }
}
