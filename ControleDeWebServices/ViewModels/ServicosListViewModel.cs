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

        [RelayCommand]
        public void Incluir()
        {
            Editor = ServicoEditViewModel.Novo();
            IsEditing = true;
        }

        [RelayCommand]
        public void Editar(ServicoListItem servico)
        {
            var item = servico ?? SelectedServico;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um servico antes de editar.", "Atencao"));
                return;
            }

            try
            {
                Editor = ServicoEditViewModel.FromEditor(servicosService.ObterParaEdicao(item.IdServicos));
                IsEditing = true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel abrir o servico. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public async Task ExcluirAsync(ServicoListItem servico)
        {
            var item = servico ?? SelectedServico;
            if (item == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um servico antes de excluir.", "Atencao"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Excluir este servico?",
                $"O servico \"{item.NomeServico}\" sera removido permanentemente.",
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
                toastService.Show(new ToastRequest(ToastKind.Success, "Servico excluido com sucesso.", "Sucesso"));
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel excluir o servico. {ex.Message}", "Erro"));
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
                toastService.Show(new ToastRequest(ToastKind.Success, "Servico salvo com sucesso.", "Sucesso"));
                Editor = null;
                IsEditing = false;
                Carregar();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Nao foi possivel salvar o servico. {ex.Message}", "Erro"));
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

        private bool ValidarEditor()
        {
            if (string.IsNullOrWhiteSpace(Editor.NomeServico))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe o nome do servico antes de salvar.", "Atencao"));
                return false;
            }

            if (string.IsNullOrWhiteSpace(Editor.Uf))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe a UF do servico antes de salvar.", "Atencao"));
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
