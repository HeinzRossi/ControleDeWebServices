using ControleDeWebServices.Application.Operacao;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Operacao
{
    public sealed class ProcessService : IProcessService
    {
        private readonly ILogger<ProcessService> logger;

        public ProcessService()
            : this(NullLogger<ProcessService>.Instance)
        {
        }

        public ProcessService(ILogger<ProcessService> logger)
        {
            this.logger = logger;
        }

        public void Start(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                logger.LogInformation("Iniciando processo externo {FileName}", fileName);
                Process.Start(fileName);
            }
        }

        public ProcessKillResult KillByPrefixes(params string[] prefixes)
        {
            var count = 0;
            var failures = new List<string>();

            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    if (prefixes.Any(prefix => process.ProcessName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                    {
                        logger.LogInformation("Encerrando processo {ProcessName} ({ProcessId})", process.ProcessName, process.Id);
                        process.Kill();
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    var message = $"{process.ProcessName}: {ex.Message}";
                    failures.Add(message);
                    logger.LogWarning(ex, "Não foi possível encerrar o processo {ProcessName} ({ProcessId})", process.ProcessName, process.Id);
                }
            }

            return new ProcessKillResult
            {
                KilledCount = count,
                Failures = failures
            };
        }
    }
}
