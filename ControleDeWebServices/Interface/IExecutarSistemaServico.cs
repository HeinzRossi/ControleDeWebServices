namespace ControleDeWebServices.Interface
{
    public interface IExecutarSistemaServico
    {
        IExecutarSistemaServico ValidarConfiguracoesSistema();
        IExecutarSistemaServico ValidarConfiguracoesServico();
        IExecutarSistemaServico AtualizarArquivoConfiguracaoSistema();
        IExecutarSistemaServico AtualizarArquivoConfiguracaoServico();
        IExecutarSistemaServico DerrubarServicos();
        IExecutarSistemaServico ExecutarServicos();
        IExecutarSistemaServico AtualizarQuantidadeDeAcessos();
        IExecutarSistemaServico AtualizarParametros();
        
    }
}
