using System.Collections.Generic;

namespace ControleDeWebServices.Application.Cadastros
{
    public interface ISistemasService
    {
        IReadOnlyList<SistemaListItem> Listar();
        SistemaEditor ObterParaEdicao(int idSistemas);
        void Salvar(SistemaEditor editor);
        void Excluir(int idSistemas);
    }
}
