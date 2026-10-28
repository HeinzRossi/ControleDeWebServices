using System.Collections.Generic;

namespace ControleDeWebServices.Application.Cadastros
{
    public interface IServicosService
    {
        IReadOnlyList<ServicoListItem> Listar();
        ServicoEditor ObterParaEdicao(int idServicos);
        void Salvar(ServicoEditor editor);
        void Excluir(int idServicos);
    }
}
