using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Feedback;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class SecoesListViewModel : ViewModelBase
    {
        private readonly ISecoesService secoesService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;

        [ObservableProperty]
        private ObservableCollection<SecaoListItem> secoes = new ObservableCollection<SecaoListItem>();

        [ObservableProperty]
        private SecaoListItem selectedSecao;

        [ObservableProperty]
        private SecaoEditViewModel editor;

        [ObservableProperty]
        private bool isEditing;

        public SecoesListViewModel(ISecoesService secoesService, IToastService toastService, IConfirmDialogService confirmDialogService)
        {
            this.secoesService = secoesService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
        }

        public string TotalRegistros => Secoes.Count == 1 ? "1 registro" : $"{Secoes.Count} registros";
        public bool IsEmpty => Secoes.Count == 0;
        public bool CanUseListActions => !IsEditing;
        public bool IsListEnabled => CanUseListActions;

        [RelayCommand]
        public void Carregar()
        {
            try
            {
                var selecionado = SelectedSecao?.IdSecao;
                Secoes = new ObservableCollection<SecaoListItem>(secoesService.Listar());
                SelectedSecao = selecionado.HasValue ? FindSecao(selecionado.Value) : null;
                if (SelectedSecao == null && Secoes.Count > 0)
                {
                    SelectedSecao = Secoes[0];
                }
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel carregar secoes. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public void Incluir()
        {
            if (IsEditing)
            {
                return;
            }

            Editor = SecaoEditViewModel.Novo();
            IsEditing = true;
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public void Editar(SecaoListItem secao)
        {
            if (IsEditing)
            {
                return;
            }

            var item = secao ?? SelectedSecao;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione uma seção antes de editar.", "Atenção"));
                return;
            }

            try
            {
                Editor = SecaoEditViewModel.FromEditor(secoesService.ObterParaEdicao(item.IdSecao));
                IsEditing = true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível abrir a seção. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand(CanExecute = nameof(CanUseListActions))]
        public async Task ExcluirAsync(SecaoListItem secao)
        {
            if (IsEditing)
            {
                return;
            }

            var item = secao ?? SelectedSecao;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione uma seção antes de excluir.", "Atenção"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Excluir esta seção?",
                $"A seção \"{item.NomeSecao}\" sera removida permanentemente.",
                "Excluir",
                "Cancelar",
                true));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                secoesService.Excluir(item.IdSecao);
                toastService.Show(new ToastRequest(ToastKind.Success, "Seção excluida com sucesso.", "Sucesso"));
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível excluir a seção. {ex.Message}", "Erro"));
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
                secoesService.Salvar(Editor.ToEditor());
                toastService.Show(new ToastRequest(ToastKind.Success, "Seção salva com sucesso.", "Sucesso"));
                Editor = null;
                IsEditing = false;
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível salvar a seção. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            Editor = null;
            IsEditing = false;
        }

        partial void OnSecoesChanged(ObservableCollection<SecaoListItem> value)
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
            if (string.IsNullOrWhiteSpace(Editor.NomeSecao))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe o nome da seção antes de salvar.", "Atenção"));
                return false;
            }

            return true;
        }

        private SecaoListItem FindSecao(int idSecao)
        {
            foreach (var secao in Secoes)
            {
                if (secao.IdSecao == idSecao)
                {
                    return secao;
                }
            }

            return null;
        }
    }
}
