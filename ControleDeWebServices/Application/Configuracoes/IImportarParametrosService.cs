using System.Collections.Generic;

namespace ControleDeWebServices.Application.Configuracoes
{
    public interface IImportarParametrosService
    {
        IReadOnlyList<string> ListarUfs(int idClientesSistemaDestino);
        IReadOnlyList<ImportacaoParametroOption> ListarClientes(string uf, int idClientesSistemaDestino);
        IReadOnlyList<ImportacaoSistemaOption> ListarSistemas(int idCliente, int idClientesSistemaDestino);
        ImportacaoParametrosResultado Importar(int idClientesSistemaOrigem, int idClientesSistemaDestino);
    }
}
