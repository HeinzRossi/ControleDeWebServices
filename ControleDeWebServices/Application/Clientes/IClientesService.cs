using System.Collections.Generic;

namespace ControleDeWebServices.Application.Clientes
{
    public interface IClientesService
    {
        IReadOnlyList<ClienteListItem> Listar();
        ClienteEditor ObterParaEdicao(int idCliente);
        void Salvar(ClienteEditor editor);
        void Excluir(int idCliente);
    }
}
