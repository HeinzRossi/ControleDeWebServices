using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Modelo;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Configuracoes
{
    public sealed class ImportarParametrosService : IImportarParametrosService
    {
        private readonly IDadosDbContextFactory contextFactory;
        private readonly ILogger<ImportarParametrosService> logger;

        public ImportarParametrosService(IDadosDbContextFactory contextFactory, ILogger<ImportarParametrosService> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        public IReadOnlyList<string> ListarUfs(int idClientesSistemaDestino)
        {
            using (var contexto = contextFactory.Create())
            {
                return ListarOrigens(contexto, idClientesSistemaDestino)
                    .Select(item => item.Uf)
                    .Distinct()
                    .OrderBy(uf => uf)
                    .ToList();
            }
        }

        public IReadOnlyList<ImportacaoParametroOption> ListarClientes(string uf, int idClientesSistemaDestino)
        {
            using (var contexto = contextFactory.Create())
            {
                return ListarOrigens(contexto, idClientesSistemaDestino)
                    .Where(item => item.Uf == uf)
                    .Select(item => new ImportacaoParametroOption
                    {
                        Id = item.IdCliente,
                        Nome = item.NomeCliente,
                        Uf = item.Uf
                    })
                    .Distinct()
                    .OrderBy(item => item.Nome)
                    .ToList();
            }
        }

        public IReadOnlyList<ImportacaoSistemaOption> ListarSistemas(int idCliente, int idClientesSistemaDestino)
        {
            using (var contexto = contextFactory.Create())
            {
                return ListarOrigens(contexto, idClientesSistemaDestino)
                    .Where(item => item.IdCliente == idCliente)
                    .Select(item => new ImportacaoSistemaOption
                    {
                        IdClientesSistema = item.IdClientesSistema,
                        NomeSistema = item.NomeSistema,
                        QuantidadeParametros = item.QuantidadeParametros
                    })
                    .OrderBy(item => item.NomeSistema)
                    .ToList();
            }
        }

        public ImportacaoParametrosResultado Importar(int idClientesSistemaOrigem, int idClientesSistemaDestino)
        {
            logger.LogInformation("Iniciando importação de parâmetros. Origem {Origem}, destino {Destino}", idClientesSistemaOrigem, idClientesSistemaDestino);

            using (var contexto = contextFactory.Create())
            {
                var parametrosOrigem = contexto.ParametrosSistema
                    .AsNoTracking()
                    .Where(parametro => parametro.IdClientesSistema == idClientesSistemaOrigem)
                    .ToList();

                if (parametrosOrigem.Count == 0)
                {
                    throw new InvalidOperationException("O sistema selecionado não possui parâmetros para importar.");
                }

                var parametrosDestino = contexto.ParametrosSistema
                    .Where(parametro => parametro.IdClientesSistema == idClientesSistemaDestino)
                    .ToList();

                var resultado = new ImportacaoParametrosResultado();

                foreach (var origem in parametrosOrigem)
                {
                    var destino = parametrosDestino.FirstOrDefault(parametro =>
                        parametro.Secao == origem.Secao &&
                        parametro.Parametro == origem.Parametro);

                    if (destino == null)
                    {
                        contexto.ParametrosSistema.Add(new ParametrosSistema
                        {
                            IdClientesSistema = idClientesSistemaDestino,
                            Secao = origem.Secao,
                            Parametro = origem.Parametro,
                            Valor = origem.Valor
                        });
                        resultado.Inseridos++;
                    }
                    else
                    {
                        destino.Valor = origem.Valor;
                        resultado.Atualizados++;
                    }
                }

                contexto.SaveChanges();
                logger.LogInformation(
                    "Importação de parâmetros concluída. Origem {Origem}, destino {Destino}, inseridos {Inseridos}, atualizados {Atualizados}",
                    idClientesSistemaOrigem,
                    idClientesSistemaDestino,
                    resultado.Inseridos,
                    resultado.Atualizados);
                return resultado;
            }
        }

        private static IQueryable<OrigemParametro> ListarOrigens(DadosDBContext contexto, int idClientesSistemaDestino)
        {
            return from clienteSistema in contexto.ClienteSistemas.AsNoTracking()
                   join cliente in contexto.Cliente.AsNoTracking() on clienteSistema.IdCliente equals cliente.IdCliente
                   join sistema in contexto.Sistemas.AsNoTracking() on clienteSistema.IdSistemas equals sistema.IdSistemas
                   let quantidade = contexto.ParametrosSistema.Count(parametro => parametro.IdClientesSistema == clienteSistema.IdClientesSistema)
                   where clienteSistema.IdClientesSistema != idClientesSistemaDestino && quantidade > 0
                   select new OrigemParametro
                   {
                       IdClientesSistema = clienteSistema.IdClientesSistema,
                       IdCliente = cliente.IdCliente,
                       NomeCliente = cliente.NomeCliente,
                       Uf = cliente.Uf,
                       NomeSistema = sistema.NomeSistema,
                       QuantidadeParametros = quantidade
                   };
        }

        private sealed class OrigemParametro
        {
            public int IdClientesSistema { get; set; }
            public int IdCliente { get; set; }
            public string NomeCliente { get; set; }
            public string Uf { get; set; }
            public string NomeSistema { get; set; }
            public int QuantidadeParametros { get; set; }
        }
    }
}
