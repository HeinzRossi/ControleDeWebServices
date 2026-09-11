using System.Collections.Generic;

namespace ControleDeWebServices.Application.Operacao
{
    public sealed class ProcessKillResult
    {
        public int KilledCount { get; set; }
        public IReadOnlyList<string> Failures { get; set; } = new List<string>();
    }
}
