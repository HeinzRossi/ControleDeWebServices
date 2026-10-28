using System.Collections.Generic;

namespace ControleDeWebServices.Application.Vinculos
{
    public interface IVinculoClienteServicoService
    {
        IReadOnlyList<string> ListarUfs();
        IReadOnlyList<ClienteVinculoListItem> ListarClientesComVinculos();
        IReadOnlyList<ClienteServicoSistemaResumo> ListarSistemasComServicosDoCliente(int idCliente);
        IReadOnlyList<ClienteServicoResumo> ListarServicosDoSistema(int idCliente, int idSistemas);
        IReadOnlyList<VinculoOptionItem> ListarClientesPorUf(string uf);
        IReadOnlyList<VinculoOptionItem> ListarSistemasDoCliente(int idCliente);
        IReadOnlyList<VinculoItem> ListarServicosDisponiveis(string uf, int idCliente, int idSistemas);
        IReadOnlyList<VinculoItem> ListarServicosVinculados(int idCliente, int idSistemas);
        void Salvar(int idCliente, int idSistemas, IEnumerable<int> servicosVinculados);
        void ExcluirVinculosDoSistema(int idCliente, int idSistemas);
    }
}
