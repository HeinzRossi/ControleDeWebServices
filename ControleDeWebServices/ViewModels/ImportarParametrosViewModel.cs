using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Feedback;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ImportarParametrosViewModel : ViewModelBase
    {
        private readonly IImportarParametrosService importarParametrosService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;

        [ObservableProperty] private int idClientesSistemaDestino;
        [ObservableProperty] private ObservableCollection<string> ufs = new ObservableCollection<string>();
        [ObservableProperty] private string selectedUf;
        [ObservableProperty] private ObservableCollection<ImportacaoParametroOption> clientes = new ObservableCollection<ImportacaoParametroOption>();
        [ObservableProperty] private ImportacaoParametroOption selectedCliente;
        [ObservableProperty] private ObservableCollection<ImportacaoSistemaOption> sistemas = new ObservableCollection<ImportacaoSistemaOption>();
        [ObservableProperty] private ImportacaoSistemaOption selectedSistema;

        public ImportarParametrosViewModel(
            IImportarParametrosService importarParametrosService,
            IToastService toastService,
            IConfirmDialogService confirmDialogService)
        {
            this.importarParametrosService = importarParametrosService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
        }

        public event EventHandler RequestClose;
        public event EventHandler ImportCompleted;

        public void Carregar(int idClientesSistemaDestino)
        {
            IdClientesSistemaDestino = idClientesSistemaDestino;
            Ufs = new ObservableCollection<string>(importarParametrosService.ListarUfs(idClientesSistemaDestino));
        }

        [RelayCommand]
        public async Task ImportarAsync()
        {
            if (SelectedSistema == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um sistema antes de importar parâmetros.", "Atenção"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Importar parâmetros?",
                $"Os parâmetros de \"{SelectedSistema.NomeSistema}\" serão copiados para esta configuração.",
                "Importar",
                "Cancelar",
                false));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                importarParametrosService.Importar(SelectedSistema.IdClientesSistema, IdClientesSistemaDestino);
                toastService.Show(new ToastRequest(ToastKind.Success, "Parâmetros importados com sucesso.", "Sucesso"));
                ImportCompleted?.Invoke(this, EventArgs.Empty);
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível importar parâmetros. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        partial void OnSelectedUfChanged(string value)
        {
            Clientes = string.IsNullOrWhiteSpace(value)
                ? new ObservableCollection<ImportacaoParametroOption>()
                : new ObservableCollection<ImportacaoParametroOption>(importarParametrosService.ListarClientes(value, IdClientesSistemaDestino));
            SelectedCliente = null;
            Sistemas = new ObservableCollection<ImportacaoSistemaOption>();
            SelectedSistema = null;
        }

        partial void OnSelectedClienteChanged(ImportacaoParametroOption value)
        {
            Sistemas = value == null
                ? new ObservableCollection<ImportacaoSistemaOption>()
                : new ObservableCollection<ImportacaoSistemaOption>(importarParametrosService.ListarSistemas(value.Id, IdClientesSistemaDestino));
            SelectedSistema = null;
        }
    }
}
