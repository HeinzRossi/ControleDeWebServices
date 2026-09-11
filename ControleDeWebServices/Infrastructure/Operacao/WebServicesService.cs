using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Application.Operacao;
using ControleDeWebServices.Diversos;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Operacao
{
    public sealed class WebServicesService : IWebServicesService
    {
        private readonly IDadosDbContextFactory contextFactory;
        private readonly IAtualizarPadroesService atualizarPadroesService;

        public WebServicesService(IDadosDbContextFactory contextFactory, IAtualizarPadroesService atualizarPadroesService)
        {
            this.contextFactory = contextFactory;
            this.atualizarPadroesService = atualizarPadroesService;
        }

        public IReadOnlyList<WebServiceItem> ListarMaisAcessados()
        {
            using (var contexto = contextFactory.Create())
            {
                return QuerySistemas(contexto)
                    .OrderByDescending(item => item.QuantidadeAcessos)
                    .Take(10)
                    .ToList();
            }
        }

        public IReadOnlyList<string> ListarUfs()
        {
            using (var contexto = contextFactory.Create())
            {
                return (from cliente in contexto.Cliente.AsNoTracking()
                        join clienteSistema in contexto.ClienteSistemas.AsNoTracking() on cliente.IdCliente equals clienteSistema.IdCliente
                        where contexto.ClienteServicos.Any(clienteServico => clienteServico.IdSistemas == clienteSistema.IdSistemas && clienteServico.IdCliente == cliente.IdCliente)
                        select cliente.Uf)
                    .Distinct()
                    .OrderBy(uf => uf)
                    .ToList();
            }
        }

        public IReadOnlyList<WebServiceClienteOption> ListarClientesPorUf(string uf)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from cliente in contexto.Cliente.AsNoTracking()
                        join clienteSistema in contexto.ClienteSistemas.AsNoTracking() on cliente.IdCliente equals clienteSistema.IdCliente
                        where cliente.Uf == uf &&
                              contexto.ClienteServicos.Any(clienteServico => clienteServico.IdSistemas == clienteSistema.IdSistemas && clienteServico.IdCliente == cliente.IdCliente)
                        select new WebServiceClienteOption
                        {
                            IdCliente = cliente.IdCliente,
                            CodigoControle = cliente.CodigoControle,
                            NomeCliente = cliente.NomeCliente,
                            Uf = cliente.Uf
                        })
                    .Distinct()
                    .OrderBy(cliente => cliente.NomeCliente)
                    .ToList();
            }
        }

        public IReadOnlyList<WebServiceItem> ListarSistemasPorCliente(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                return QuerySistemas(contexto)
                    .Where(item => item.IdCliente == idCliente)
                    .OrderBy(item => item.NomeSistema)
                    .ToList();
            }
        }

        public IReadOnlyList<WebServiceItem> ListarSistemasPorCodigoCliente(int codigoControle)
        {
            using (var contexto = contextFactory.Create())
            {
                return QuerySistemas(contexto)
                    .Where(item => item.CodigoControle == codigoControle)
                    .OrderBy(item => item.NomeSistema)
                    .ToList();
            }
        }

        public AtualizarUrlResult AtualizarUrl(int idClientesSistema)
        {
            using (var contexto = contextFactory.Create())
            {
                var clienteSistema = contexto.ClienteSistemas.Find(idClientesSistema);
                if (clienteSistema == null)
                {
                    return AtualizarUrlResult.Warning("WebService não encontrado para atualizar a URL.");
                }

                return atualizarPadroesService.AtualizarUrl(clienteSistema);
            }
        }

        private static IQueryable<WebServiceItem> QuerySistemas(DadosDBContext contexto)
        {
            return from sistema in contexto.Sistemas.AsNoTracking()
                   join clienteSistema in contexto.ClienteSistemas.AsNoTracking() on sistema.IdSistemas equals clienteSistema.IdSistemas
                   join cliente in contexto.Cliente.AsNoTracking() on clienteSistema.IdCliente equals cliente.IdCliente
                   where contexto.ClienteServicos.Any(clienteServico => clienteServico.IdSistemas == sistema.IdSistemas && clienteServico.IdCliente == cliente.IdCliente)
                   select new WebServiceItem
                   {
                       IdClientesSistema = clienteSistema.IdClientesSistema,
                       IdCliente = clienteSistema.IdCliente,
                       IdSistemas = clienteSistema.IdSistemas,
                       CodigoControle = cliente.CodigoControle,
                       NomeCliente = cliente.NomeCliente,
                       NomeSistema = sistema.NomeSistema,
                       Uf = cliente.Uf,
                       DataBase = clienteSistema.DataBase,
                       ArquivoConfiguracao = clienteSistema.ArquivoConfiguracao,
                       ArquivoExecutavel = clienteSistema.ArquivoExecutavel,
                       Porta = clienteSistema.Porta,
                       Servidor = clienteSistema.Servidor,
                       TipoConexao = clienteSistema.TipoConexao,
                       Usuario = clienteSistema.Usuario,
                       QuantidadeAcessos = clienteSistema.QuantidadeAcessos
                   };
        }
    }
}
