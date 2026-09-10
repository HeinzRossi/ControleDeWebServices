using System.Collections.Generic;

namespace ControleDeWebServices.Application.Operacao
{
    public sealed class WebServiceExecutionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IReadOnlyList<string> CompletedSteps { get; set; }
    }
}
