using System.Collections.Generic;

namespace ControleDeWebServices.Application.Cadastros
{
    public interface ISecoesService
    {
        IReadOnlyList<SecaoListItem> Listar();
        SecaoEditor ObterParaEdicao(int idSecao);
        void Salvar(SecaoEditor editor);
        void Excluir(int idSecao);
    }
}
