using System.Collections.Generic;

namespace ControleDeWebServices.Application.Operacao
{
    public interface IWebServicesService
    {
        IReadOnlyList<WebServiceItem> ListarMaisAcessados();
        IReadOnlyList<string> ListarUfs();
        IReadOnlyList<WebServiceClienteOption> ListarClientesPorUf(string uf);
        IReadOnlyList<WebServiceItem> ListarSistemasPorCliente(int idCliente);
        IReadOnlyList<WebServiceItem> ListarSistemasPorCodigoCliente(int codigoControle);
        void AtualizarUrl(int idClientesSistema);
    }
}
