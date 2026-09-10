using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Application.Platform;
using System;
using System.Collections.ObjectModel;

namespace ControleDeWebServices.ViewModels
{
    public sealed partial class ConfiguracaoServicoViewModel : ViewModelBase
    {
        private readonly IConfiguracaoServicoService configuracaoService;
        private readonly IFilePickerService filePickerService;
        private readonly IToastService toastService;

        [ObservableProperty] private int idClienteServico;
        [ObservableProperty] private string uf;
        [ObservableProperty] private string nomeCliente;
        [ObservableProperty] private string nomeSistema;
        [ObservableProperty] private string nomeServico;
        [ObservableProperty] private string arquivoExecutavel;
        [ObservableProperty] private string arquivoConfiguracao;
        [ObservableProperty] private string servidor;
        [ObservableProperty] private int porta;
        [ObservableProperty] private string dataBase;
        [ObservableProperty] private int tipoConexao;
        [ObservableProperty] private string usuario;
        [ObservableProperty] private string senha;
        [ObservableProperty] private ObservableCollection<TipoConexaoOption> tiposConexao = new ObservableCollection<TipoConexaoOption>();

        public ConfiguracaoServicoViewModel(
            IConfiguracaoServicoService configuracaoService,
            IFilePickerService filePickerService,
            IToastService toastService)
        {
            this.configuracaoService = configuracaoService;
            this.filePickerService = filePickerService;
            this.toastService = toastService;
        }

        public event EventHandler RequestClose;

        public void Carregar(int idClienteServico)
        {
            try
            {
                var editor = configuracaoService.Obter(idClienteServico);
                IdClienteServico = editor.IdClienteServico;
                Uf = editor.Uf;
                NomeCliente = editor.NomeCliente;
                NomeSistema = editor.NomeSistema;
                NomeServico = editor.NomeServico;
                ArquivoExecutavel = editor.ArquivoExecutavel;
                ArquivoConfiguracao = editor.ArquivoConfiguracao;
                Servidor = editor.Servidor;
                Porta = editor.Porta;
                DataBase = editor.DataBase;
                TipoConexao = editor.TipoConexao;
                Usuario = editor.Usuario;
                Senha = editor.Senha;
                TiposConexao = new ObservableCollection<TipoConexaoOption>(configuracaoService.ListarTiposConexao());
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível carregar a configuração do serviço. {ex.Message}", "Erro"));
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
        public void Salvar()
        {
            if (TrySalvar())
            {
                RequestClose?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool TrySalvar()
        {
            if (string.IsNullOrWhiteSpace(NomeCliente) || string.IsNullOrWhiteSpace(NomeSistema) || string.IsNullOrWhiteSpace(NomeServico))
            {
                toastService.Show(new ToastRequest(ToastKind.Warning, "Configuração sem cliente, sistema ou serviço selecionado.", "Atenção"));
                return false;
            }

            try
            {
                configuracaoService.Salvar(ToEditor());
                toastService.Show(new ToastRequest(ToastKind.Success, "Configuração do serviço salva com sucesso.", "Sucesso"));
                return true;
            }
            catch (Exception ex)
            {
                toastService.Show(new ToastRequest(ToastKind.Error, $"Não foi possível salvar a configuração do serviço. {ex.Message}", "Erro"));
                return false;
            }
        }

        [RelayCommand]
        public void Cancelar()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        private ConfiguracaoServicoEditor ToEditor()
        {
            return new ConfiguracaoServicoEditor
            {
                IdClienteServico = IdClienteServico,
                Uf = Uf,
                NomeCliente = NomeCliente,
                NomeSistema = NomeSistema,
                NomeServico = NomeServico,
                ArquivoExecutavel = ArquivoExecutavel,
                ArquivoConfiguracao = ArquivoConfiguracao,
                Servidor = Servidor,
                Porta = Porta,
                DataBase = DataBase,
                TipoConexao = TipoConexao,
                Usuario = Usuario,
                Senha = Senha
            };
        }
    }
}
