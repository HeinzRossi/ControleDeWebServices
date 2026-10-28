using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Feedback;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class SistemasListViewModel : ViewModelBase
    {
        private readonly ISistemasService sistemasService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;

        [ObservableProperty]
        private ObservableCollection<SistemaListItem> sistemas = new ObservableCollection<SistemaListItem>();

        [ObservableProperty]
        private SistemaListItem selectedSistema;

        [ObservableProperty]
        private SistemaEditViewModel editor;

        [ObservableProperty]
        private bool isEditing;

        public SistemasListViewModel(ISistemasService sistemasService, IToastService toastService, IConfirmDialogService confirmDialogService)
        {
            this.sistemasService = sistemasService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
        }

        public string TotalRegistros => Sistemas.Count == 1 ? "1 registro" : $"{Sistemas.Count} registros";
        public bool IsEmpty => Sistemas.Count == 0;
        public bool CanUseListActions => !IsEditing;
        public bool IsListEnabled => CanUseListActions;

        [RelayCommand]
        public void Carregar()
        {
            try
            {
                var selecionado = SelectedSistema?.IdSistemas;
                Sistemas = new ObservableCollection<SistemaListItem>(sistemasService.Listar());
                SelectedSistema = selecionado.HasValue ? FindSistema(selecionado.Value) : null;
                if (SelectedSistema == null && Sistemas.Count > 0)
                {
                    SelectedSistema = Sistemas[0];
                }
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel carregar sistemas. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public void Incluir()
        {
            if (IsEditing)
            {
                return;
            }

            Editor = SistemaEditViewModel.Novo();
            IsEditing = true;
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public void Editar(SistemaListItem sistema)
        {
            if (IsEditing)
            {
                return;
            }

            var item = sistema ?? SelectedSistema;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um sistema antes de editar.", "Atenção"));
                return;
            }

            try
            {
                Editor = SistemaEditViewModel.FromEditor(sistemasService.ObterParaEdicao(item.IdSistemas));
                IsEditing = true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível abrir o sistema. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public async Task ExcluirAsync(SistemaListItem sistema)
        {
            if (IsEditing)
            {
                return;
            }

            var item = sistema ?? SelectedSistema;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um sistema antes de excluir.", "Atenção"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Excluir este sistema?",
                $"O sistema \"{item.NomeSistema}\" sera removido permanentemente.",
                "Excluir",
                "Cancelar",
                true));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                sistemasService.Excluir(item.IdSistemas);
                toastService.Show(new ToastRequest(ToastKind.Success, "Sistema excluido com sucesso.", "Sucesso"));
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel excluir o sistema. {ex.Message}", "Erro"));
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
                sistemasService.Salvar(Editor.ToEditor());
                toastService.Show(new ToastRequest(ToastKind.Success, "Sistema salvo com sucesso.", "Sucesso"));
                Editor = null;
                IsEditing = false;
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel salvar o sistema. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            Editor = null;
            IsEditing = false;
        }

        partial void OnSistemasChanged(ObservableCollection<SistemaListItem> value)
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
            if (string.IsNullOrWhiteSpace(Editor.NomeSistema))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe o nome do sistema antes de salvar.", "Atenção"));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Editor.Uf))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe a UF do sistema antes de salvar.", "Atenção"));
                return false;
            }

            return true;
        }

        private SistemaListItem FindSistema(int idSistemas)
        {
            foreach (var sistema in Sistemas)
            {
                if (sistema.IdSistemas == idSistemas)
                {
                    return sistema;
                }
            }

            return null;
        }
    }
}
