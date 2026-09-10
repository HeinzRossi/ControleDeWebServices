using System.Collections.Generic;

namespace ControleDeWebServices.Application.Configuracoes
{
    public interface IConfiguracaoServicoService
    {
        ConfiguracaoServicoEditor Obter(int idClienteServico);
        IReadOnlyList<TipoConexaoOption> ListarTiposConexao();
        void Salvar(ConfiguracaoServicoEditor editor);
    }
}
