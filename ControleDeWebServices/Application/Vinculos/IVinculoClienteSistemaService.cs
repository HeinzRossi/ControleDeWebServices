using System.Collections.Generic;

namespace ControleDeWebServices.Application.Vinculos
{
    public interface IVinculoClienteSistemaService
    {
        IReadOnlyList<string> ListarUfs();
        IReadOnlyList<ClienteVinculoListItem> ListarClientesComVinculos();
        IReadOnlyList<ClienteSistemaResumo> ListarSistemasDoCliente(int idCliente);
        IReadOnlyList<VinculoOptionItem> ListarClientesPorUf(string uf, bool somenteSemVinculo);
        IReadOnlyList<VinculoItem> ListarSistemasDisponiveis(string uf, int idCliente);
        IReadOnlyList<VinculoItem> ListarSistemasVinculados(int idCliente);
        void Salvar(int idCliente, IEnumerable<int> sistemasVinculados);
        void ExcluirVinculosDoCliente(int idCliente);
    }
}
