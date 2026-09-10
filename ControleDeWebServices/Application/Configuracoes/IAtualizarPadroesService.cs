using ControleDeWebServices.Modelo;
using System.Collections.Generic;

namespace ControleDeWebServices.Application.Configuracoes
{
    public interface IAtualizarPadroesService
    {
        void AtualizarParametros(ClienteSistemas clienteSistemas, IEnumerable<ParametroEditor> parametros);
        void AtualizarUrl(ClienteSistemas clienteSistemas);
    }
}
