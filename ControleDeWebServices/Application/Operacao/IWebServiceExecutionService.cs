using System;
using System.Threading.Tasks;

namespace ControleDeWebServices.Application.Operacao
{
    public interface IWebServiceExecutionService
    {
        Task<WebServiceExecutionResult> ExecutarAsync(int idClientesSistema, Action<WebServiceExecutionStep> progress);
    }
}
