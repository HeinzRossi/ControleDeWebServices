using ControleDeWebServices.Modelo;

namespace ControleDeWebServices.Interface
{
    public interface IAtualizarPadroes
    {
        IAtualizarPadroes AtualizarURL(ClienteSistemas pclienteSistemas);
        IAtualizarPadroes AtualizarParametros(ClienteSistemas pclienteSistemas, DadosDBContext pContexto);
    }
}
