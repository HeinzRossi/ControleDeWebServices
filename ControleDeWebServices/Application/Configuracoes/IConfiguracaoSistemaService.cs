using System.Collections.Generic;

namespace ControleDeWebServices.Application.Configuracoes
{
    public interface IConfiguracaoSistemaService
    {
        ConfiguracaoSistemaEditor Obter(int idClientesSistema);
        IReadOnlyList<TipoConexaoOption> ListarTiposConexao();
        IReadOnlyList<SecaoOption> ListarSecoes();
        void Salvar(ConfiguracaoSistemaEditor editor);
    }
}
