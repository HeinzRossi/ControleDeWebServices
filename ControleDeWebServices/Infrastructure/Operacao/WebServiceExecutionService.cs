using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Application.Operacao;
using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ControleDeWebServices.Infrastructure.Operacao
{
    public sealed class WebServiceExecutionService : IWebServiceExecutionService
    {
        private readonly IDadosDbContextFactory contextFactory;
        private readonly IProcessService processService;
        private readonly ILogger<WebServiceExecutionService> logger;

        public WebServiceExecutionService(
            IDadosDbContextFactory contextFactory,
            IProcessService processService,
            ILogger<WebServiceExecutionService> logger)
        {
            this.contextFactory = contextFactory;
            this.processService = processService;
            this.logger = logger;
        }

        public Task<WebServiceExecutionResult> ExecutarAsync(int idClientesSistema, Action<WebServiceExecutionStep> progress)
        {
            return Task.Run(() =>
            {
                var result = new WebServiceExecutionResult
                {
                    Success = false,
                    CompletedSteps = Array.Empty<string>()
                };

                var steps = new System.Collections.Generic.List<string>();

                try
                {
                    logger.LogInformation("Iniciando execução de WebService {IdClientesSistema}", idClientesSistema);

                    using (var contexto = contextFactory.Create())
                    {
                        var clienteSistema = CarregarClienteSistema(contexto, idClientesSistema);
                        var executor = new ExecutarSistemaServico(clienteSistema, contexto, processService);

                        RunStep("Validando configuração do sistema", steps, progress, () => executor.ValidarConfiguracoesSistema());
                        RunStep("Validando serviços vinculados", steps, progress, () => executor.ValidarConfiguracoesServico());
                        RunStep("Atualizando arquivo de configuração do sistema", steps, progress, () => executor.AtualizarArquivoConfiguracaoSistema());
                        RunStep("Atualizando arquivos de serviços", steps, progress, () => executor.AtualizarArquivoConfiguracaoServico());
                        RunStep("Atualizando parâmetros externos", steps, progress, () => executor.AtualizarParametros());
                        RunStep("Encerrando serviços em execução", steps, progress, () => executor.DerrubarServicos());
                        RunStep("Iniciando serviços", steps, progress, () => executor.ExecutarServicos());
                        RunStep("Registrando acesso", steps, progress, () => executor.AtualizarQuantidadeDeAcessos());
                    }

                    result.Success = true;
                    result.Message = "WebService executado com sucesso.";
                    result.CompletedSteps = steps;
                    logger.LogInformation("WebService {IdClientesSistema} executado com sucesso com {StepCount} etapas", idClientesSistema, steps.Count);
                    return result;
                }
                catch (Exception ex)
                {
                    result.Message = $"Falha na execução do WebService. {ex.Message}";
                    result.CompletedSteps = steps;
                    logger.LogError(ex, "Falha na execução de WebService {IdClientesSistema} após {StepCount} etapas", idClientesSistema, steps.Count);
                    return result;
                }
            });
        }

        private static void RunStep(string name, System.Collections.Generic.List<string> steps, Action<WebServiceExecutionStep> progress, Action action)
        {
            progress?.Invoke(new WebServiceExecutionStep(name));
            action();
            steps.Add(name);
        }

        private static ClienteSistemas CarregarClienteSistema(DadosDBContext contexto, int idClientesSistema)
        {
            var clienteSistema = contexto.ClienteSistemas.Find(idClientesSistema);
            if (clienteSistema == null)
            {
                throw new InvalidOperationException("WebService selecionado não foi encontrado.");
            }

            var dados = (from cliente in contexto.Cliente.AsNoTracking()
                         join sistema in contexto.Sistemas.AsNoTracking() on clienteSistema.IdSistemas equals sistema.IdSistemas
                         where cliente.IdCliente == clienteSistema.IdCliente
                         select new
                         {
                             cliente.NomeCliente,
                             cliente.Uf,
                             sistema.NomeSistema
                         }).FirstOrDefault();

            if (dados != null)
            {
                clienteSistema.NomeCliente = dados.NomeCliente;
                clienteSistema.Uf = dados.Uf;
                clienteSistema.NomeSistema = dados.NomeSistema;
            }

            return clienteSistema;
        }
    }
}
