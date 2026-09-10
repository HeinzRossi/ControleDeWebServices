using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Platform;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ConfiguracaoSistemaViewModel : ViewModelBase
    {
        private readonly IConfiguracaoSistemaService configuracaoService;
        private readonly IImportarParametrosService importarParametrosService;
        private readonly IFilePickerService filePickerService;
        private readonly IToastService toastService;
        private readonly IConfirmDialogService confirmDialogService;

        [ObservableProperty] private int idClientesSistema;
        [ObservableProperty] private string uf;
        [ObservableProperty] private string nomeCliente;
        [ObservableProperty] private string nomeSistema;
        [ObservableProperty] private string arquivoExecutavel;
        [ObservableProperty] private string arquivoConfiguracao;
        [ObservableProperty] private string servidor;
        [ObservableProperty] private int porta;
        [ObservableProperty] private string dataBase;
        [ObservableProperty] private int tipoConexao;
        [ObservableProperty] private string usuario;
        [ObservableProperty] private string senha;
        [ObservableProperty] private int idSecao;
        [ObservableProperty] private bool usaCriptografia;
        [ObservableProperty] private ObservableCollection<TipoConexaoOption> tiposConexao = new ObservableCollection<TipoConexaoOption>();
        [ObservableProperty] private ObservableCollection<SecaoOption> secoes = new ObservableCollection<SecaoOption>();
        [ObservableProperty] private ObservableCollection<ParametroEditor> parametros = new ObservableCollection<ParametroEditor>();
        [ObservableProperty] private ParametroEditor selectedParametro;
        [ObservableProperty] private string novaSecao;
        [ObservableProperty] private string novoParametro;
        [ObservableProperty] private string novoValor;
        [ObservableProperty] private ObservableCollection<string> importacaoUfs = new ObservableCollection<string>();
        [ObservableProperty] private string selectedImportacaoUf;
        [ObservableProperty] private ObservableCollection<ImportacaoParametroOption> importacaoClientes = new ObservableCollection<ImportacaoParametroOption>();
        [ObservableProperty] private ImportacaoParametroOption selectedImportacaoCliente;
        [ObservableProperty] private ObservableCollection<ImportacaoSistemaOption> importacaoSistemas = new ObservableCollection<ImportacaoSistemaOption>();
        [ObservableProperty] private ImportacaoSistemaOption selectedImportacaoSistema;

        public ConfiguracaoSistemaViewModel(
            IConfiguracaoSistemaService configuracaoService,
            IImportarParametrosService importarParametrosService,
            IFilePickerService filePickerService,
            IToastService toastService,
            IConfirmDialogService confirmDialogService)
        {
            this.configuracaoService = configuracaoService;
            this.importarParametrosService = importarParametrosService;
            this.filePickerService = filePickerService;
            this.toastService = toastService;
            this.confirmDialogService = confirmDialogService;
        }

        public event EventHandler RequestClose;

        public void Carregar(int idClientesSistema)
        {
            try
            {
                var editor = configuracaoService.Obter(idClientesSistema);
                IdClientesSistema = editor.IdClientesSistema;
                Uf = editor.Uf;
                NomeCliente = editor.NomeCliente;
                NomeSistema = editor.NomeSistema;
                ArquivoExecutavel = editor.ArquivoExecutavel;
                ArquivoConfiguracao = editor.ArquivoConfiguracao;
                Servidor = editor.Servidor;
                Porta = editor.Porta;
                DataBase = editor.DataBase;
                TipoConexao = editor.TipoConexao;
                Usuario = editor.Usuario;
                Senha = editor.Senha;
                IdSecao = editor.IdSecao;
                UsaCriptografia = editor.UsaCriptografia;
                Parametros = new ObservableCollection<ParametroEditor>(editor.Parametros ?? Array.Empty<ParametroEditor>());
                TiposConexao = new ObservableCollection<TipoConexaoOption>(configuracaoService.ListarTiposConexao());
                Secoes = new ObservableCollection<SecaoOption>(configuracaoService.ListarSecoes());
                CarregarImportacao();
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível carregar a configuração. {ex.Message}", "Erro"));
            }
        }

        [RelayCommand]
        public void SelecionarArquivoExecutavel()
        {
            var arquivo = filePickerService.PickFile("Selecionar arquivo executável");
            if (!string.IsNullOrWhiteSpace(arquivo))
            {
                ArquivoExecutavel = arquivo;
            }
        }

        [RelayCommand]
        public void SelecionarArquivoConfiguracao()
        {
            var arquivo = filePickerService.PickFile("Selecionar arquivo de configuração");
            if (!string.IsNullOrWhiteSpace(arquivo))
            {
                ArquivoConfiguracao = arquivo;
            }
        }

        [RelayCommand]
        public void AdicionarParametro()
        {
            if (string.IsNullOrWhiteSpace(NovaSecao) || string.IsNullOrWhiteSpace(NovoParametro))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Informe seção e parâmetro antes de adicionar.", "Atenção"));
                return;
            }

            if (Parametros.Any(item => string.Equals(item.Secao, NovaSecao, StringComparison.OrdinalIgnoreCase) && string.Equals(item.Parametro, NovoParametro, StringComparison.OrdinalIgnoreCase)))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Este parâmetro já existe na lista.", "Atenção"));
                return;
            }

            Parametros.Add(new ParametroEditor
            {
                IdClientesSistema = IdClientesSistema,
                Secao = NovaSecao.Trim(),
                Parametro = NovoParametro.Trim(),
                Valor = NovoValor?.Trim()
            });

            NovaSecao = string.Empty;
            NovoParametro = string.Empty;
            NovoValor = string.Empty;
        }

        [RelayCommand]
        public async Task RemoverParametroAsync()
        {
            if (SelectedParametro == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um parâmetro antes de remover.", "Atenção"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Remover parâmetro?",
                $"O parâmetro \"{SelectedParametro.Parametro}\" será removido desta configuração.",
                "Remover",
                "Cancelar",
                true));

            if (result == ConfirmDialogResult.Confirmed)
            {
                Parametros.Remove(SelectedParametro);
            }
        }

        [RelayCommand]
        public void Salvar()
        {
            if (TrySalvar())
            {
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool TrySalvar()
        {
            if (string.IsNullOrWhiteSpace(NomeCliente) || string.IsNullOrWhiteSpace(NomeSistema))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Configuração sem cliente ou sistema selecionado.", "Atenção"));
                return false;
            }

            try
            {
                configuracaoService.Salvar(ToEditor());
                toastService.Show(new ToastRequest(ToastKind.Success, "Configuração salva com sucesso.", "Sucesso"));
                return true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível salvar a configuração. {ex.Message}", "Erro"));
                return false;
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand]
        public void CarregarImportacao()
        {
            ImportacaoUfs = new ObservableCollection<string>(importarParametrosService.ListarUfs(IdClientesSistema));
            SelectedImportacaoUf = null;
            ImportacaoClientes = new ObservableCollection<ImportacaoParametroOption>();
            SelectedImportacaoCliente = null;
            ImportacaoSistemas = new ObservableCollection<ImportacaoSistemaOption>();
            SelectedImportacaoSistema = null;
        }

        [RelayCommand]
        public async Task ImportarParametrosAsync()
        {
            if (SelectedImportacaoSistema == null)
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Selecione um sistema antes de importar parâmetros.", "Atenção"));
                return;
            }

            var result = await confirmDialogService.ConfirmAsync(new ConfirmDialogRequest(
                "Importar parâmetros?",
                $"Os parâmetros de \"{SelectedImportacaoSistema.NomeSistema}\" serão copiados para esta configuração.",
                "Importar",
                "Cancelar",
                false));

            if (result != ConfirmDialogResult.Confirmed)
            {
                return;
            }

            try
            {
                var importacao = importarParametrosService.Importar(SelectedImportacaoSistema.IdClientesSistema, IdClientesSistema);
                Carregar(IdClientesSistema);
                toastService.Show(new ToastRequest(ToastKind.Success, $"Parâmetros importados com sucesso. {importacao.Inseridos} inseridos, {importacao.Atualizados} atualizados.", "Sucesso"));
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível importar parâmetros. {ex.Message}", "Erro"));
            }
        }

        partial void OnSelectedImportacaoUfChanged(string value)
        {
            ImportacaoClientes = string.IsNullOrWhiteSpace(value)
                ? new ObservableCollection<ImportacaoParametroOption>()
                : new ObservableCollection<ImportacaoParametroOption>(importarParametrosService.ListarClientes(value, IdClientesSistema));
            SelectedImportacaoCliente = null;
            ImportacaoSistemas = new ObservableCollection<ImportacaoSistemaOption>();
            SelectedImportacaoSistema = null;
        }

        partial void OnSelectedImportacaoClienteChanged(ImportacaoParametroOption value)
        {
            ImportacaoSistemas = value == null
                ? new ObservableCollection<ImportacaoSistemaOption>()
                : new ObservableCollection<ImportacaoSistemaOption>(importarParametrosService.ListarSistemas(value.Id, IdClientesSistema));
            SelectedImportacaoSistema = null;
        }

        private ConfiguracaoSistemaEditor ToEditor()
        {
            return new ConfiguracaoSistemaEditor
            {
                IdClientesSistema = IdClientesSistema,
                Uf = Uf,
                NomeCliente = NomeCliente,
                NomeSistema = NomeSistema,
                ArquivoExecutavel = ArquivoExecutavel,
                ArquivoConfiguracao = ArquivoConfiguracao,
                Servidor = Servidor,
                Porta = Porta,
                DataBase = DataBase,
                TipoConexao = TipoConexao,
                Usuario = Usuario,
                Senha = Senha,
                IdSecao = IdSecao,
                UsaCriptografia = UsaCriptografia,
                Parametros = Parametros.ToList()
            };
        }
    }
}
