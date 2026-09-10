using ControleDeWebServices.Application.Operacao;
using System;
using System.Diagnostics;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Operacao
{
    public sealed class ProcessService : IProcessService
    {
        public void Start(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                Process.Start(fileName);
            }
        }

        public int KillByPrefixes(params string[] prefixes)
        {
            var count = 0;
            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (prefixes.Any(prefix => process.ProcessName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                    {
                        process.Kill();
                        count++;
                    }
                }
                catch
                {
                    // Processos podem encerrar ou negar acesso entre enumeração e kill.
                }
            }

            return count;
        }
    }
}
