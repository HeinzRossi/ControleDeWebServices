using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Application.Vinculos;
using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Vinculos
{
    public sealed class VinculoClienteServicoService : IVinculoClienteServicoService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public VinculoClienteServicoService(IDadosDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IReadOnlyList<string> ListarUfs()
        {
            return Funcoes.GetEnumValues().Select(estado => estado.ToString()).ToList();
        }

        public IReadOnlyList<ClienteVinculoListItem> ListarClientesComVinculos()
        {
            using (var contexto = contextFactory.Create())
            {
                return (from cliente in contexto.Cliente.AsNoTracking()
                        join clienteServico in contexto.ClienteServicos.AsNoTracking() on cliente.IdCliente equals clienteServico.IdCliente
                        select new ClienteVinculoListItem
                        {
                            IdCliente = cliente.IdCliente,
                            NomeCliente = cliente.NomeCliente,
                            Uf = cliente.Uf
                        })
                    .Distinct()
                    .OrderBy(cliente => cliente.NomeCliente)
                    .ToList();
            }
        }

        public IReadOnlyList<ClienteServicoSistemaResumo> ListarSistemasComServicosDoCliente(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from clienteServico in contexto.ClienteServicos.AsNoTracking()
                        join sistema in contexto.Sistemas.AsNoTracking() on clienteServico.IdSistemas equals sistema.IdSistemas
                        join cliente in contexto.Cliente.AsNoTracking() on clienteServico.IdCliente equals cliente.IdCliente
                        where clienteServico.IdCliente == idCliente
                        select new ClienteServicoSistemaResumo
                        {
                            IdCliente = cliente.IdCliente,
                            IdSistemas = sistema.IdSistemas,
                            NomeCliente = cliente.NomeCliente,
                            NomeSistema = sistema.NomeSistema,
                            Uf = sistema.Uf
                        })
                    .Distinct()
                    .OrderBy(sistema => sistema.NomeSistema)
                    .ToList();
            }
        }

        public IReadOnlyList<ClienteServicoResumo> ListarServicosDoSistema(int idCliente, int idSistemas)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from clienteServico in contexto.ClienteServicos.AsNoTracking()
                        join cliente in contexto.Cliente.AsNoTracking() on clienteServico.IdCliente equals cliente.IdCliente
                        join sistema in contexto.Sistemas.AsNoTracking() on clienteServico.IdSistemas equals sistema.IdSistemas
                        join servico in contexto.Servicos.AsNoTracking() on clienteServico.IdServicos equals servico.IdServicos
                        where clienteServico.IdCliente == idCliente && clienteServico.IdSistemas == idSistemas
                        orderby servico.NomeServico
                        select new ClienteServicoResumo
                        {
                            IdClienteServico = clienteServico.IdClienteServico,
                            IdCliente = clienteServico.IdCliente,
                            IdSistemas = clienteServico.IdSistemas,
                            IdServicos = clienteServico.IdServicos,
                            NomeCliente = cliente.NomeCliente,
                            NomeSistema = sistema.NomeSistema,
                            NomeServico = servico.NomeServico,
                            Uf = servico.Uf
                        }).ToList();
            }
        }

        public IReadOnlyList<VinculoOptionItem> ListarClientesPorUf(string uf)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from cliente in contexto.Cliente.AsNoTracking()
                        join clienteSistema in contexto.ClienteSistemas.AsNoTracking() on cliente.IdCliente equals clienteSistema.IdCliente
                        where cliente.Uf == uf
                        select new VinculoOptionItem
                        {
                            Id = cliente.IdCliente,
                            Nome = cliente.NomeCliente,
                            Uf = cliente.Uf
                        })
                    .Distinct()
                    .OrderBy(cliente => cliente.Nome)
                    .ToList();
            }
        }

        public IReadOnlyList<VinculoOptionItem> ListarSistemasDoCliente(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from clienteSistema in contexto.ClienteSistemas.AsNoTracking()
                        join sistema in contexto.Sistemas.AsNoTracking() on clienteSistema.IdSistemas equals sistema.IdSistemas
                        where clienteSistema.IdCliente == idCliente
                        orderby sistema.NomeSistema
                        select new VinculoOptionItem
                        {
                            Id = sistema.IdSistemas,
                            Nome = sistema.NomeSistema,
                            Uf = sistema.Uf
                        }).ToList();
            }
        }

        public IReadOnlyList<VinculoItem> ListarServicosDisponiveis(string uf, int idCliente, int idSistemas)
        {
            using (var contexto = contextFactory.Create())
            {
                var vinculados = contexto.ClienteServicos
                    .Where(item => item.IdCliente == idCliente && item.IdSistemas == idSistemas)
                    .Select(item => item.IdServicos);

                return contexto.Servicos
                    .AsNoTracking()
                    .Where(servico => servico.Uf == uf && !vinculados.Contains(servico.IdServicos))
                    .OrderBy(servico => servico.NomeServico)
                    .Select(servico => new VinculoItem
                    {
                        Id = servico.IdServicos,
                        Nome = servico.NomeServico,
                        Uf = servico.Uf
                    })
                    .ToList();
            }
        }

        public IReadOnlyList<VinculoItem> ListarServicosVinculados(int idCliente, int idSistemas)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from clienteServico in contexto.ClienteServicos.AsNoTracking()
                        join servico in contexto.Servicos.AsNoTracking() on clienteServico.IdServicos equals servico.IdServicos
                        where clienteServico.IdCliente == idCliente && clienteServico.IdSistemas == idSistemas
                        orderby servico.NomeServico
                        select new VinculoItem
                        {
                            Id = servico.IdServicos,
                            Nome = servico.NomeServico,
                            Uf = servico.Uf
                        }).ToList();
            }
        }

        public void Salvar(int idCliente, int idSistemas, IEnumerable<int> servicosVinculados)
        {
            using (var contexto = contextFactory.Create())
            {
                var idsDesejados = new HashSet<int>(servicosVinculados);
                var vinculosAtuais = contexto.ClienteServicos
                    .Where(item => item.IdCliente == idCliente && item.IdSistemas == idSistemas)
                    .ToList();
                var idsAtuais = new HashSet<int>(vinculosAtuais.Select(item => item.IdServicos));

                foreach (var vinculo in vinculosAtuais.Where(item => !idsDesejados.Contains(item.IdServicos)).ToList())
                {
                    contexto.ClienteServicos.Remove(vinculo);
                }

                foreach (var idServico in idsDesejados.Where(idServico => !idsAtuais.Contains(idServico)))
                {
                    contexto.ClienteServicos.Add(new ClienteServicos
                    {
                        IdCliente = idCliente,
                        IdSistemas = idSistemas,
                        IdServicos = idServico
                    });
                }

                contexto.SaveChanges();
            }
        }

        public void ExcluirVinculosDoSistema(int idCliente, int idSistemas)
        {
            using (var contexto = contextFactory.Create())
            {
                var vinculos = contexto.ClienteServicos
                    .Where(item => item.IdCliente == idCliente && item.IdSistemas == idSistemas)
                    .ToList();

                foreach (var vinculo in vinculos)
                {
                    contexto.ClienteServicos.Remove(vinculo);
                }

                contexto.SaveChanges();
            }
        }
    }
}
