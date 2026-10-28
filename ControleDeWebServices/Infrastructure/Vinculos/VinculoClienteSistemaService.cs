using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Application.Vinculos;
using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Vinculos
{
    public sealed class VinculoClienteSistemaService : IVinculoClienteSistemaService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public VinculoClienteSistemaService(IDadosDbContextFactory contextFactory)
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
                        join clienteSistema in contexto.ClienteSistemas.AsNoTracking() on cliente.IdCliente equals clienteSistema.IdCliente
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

        public IReadOnlyList<ClienteSistemaResumo> ListarSistemasDoCliente(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from clienteSistema in contexto.ClienteSistemas.AsNoTracking()
                        join sistema in contexto.Sistemas.AsNoTracking() on clienteSistema.IdSistemas equals sistema.IdSistemas
                        join cliente in contexto.Cliente.AsNoTracking() on clienteSistema.IdCliente equals cliente.IdCliente
                        where clienteSistema.IdCliente == idCliente
                        orderby sistema.NomeSistema
                        select new ClienteSistemaResumo
                        {
                            IdClientesSistema = clienteSistema.IdClientesSistema,
                            IdCliente = clienteSistema.IdCliente,
                            IdSistemas = clienteSistema.IdSistemas,
                            NomeCliente = cliente.NomeCliente,
                            NomeSistema = sistema.NomeSistema,
                            Uf = sistema.Uf
                        }).ToList();
            }
        }

        public IReadOnlyList<VinculoOptionItem> ListarClientesPorUf(string uf, bool somenteSemVinculo)
        {
            using (var contexto = contextFactory.Create())
            {
                var clientesVinculados = contexto.ClienteSistemas.Select(item => item.IdCliente);
                var query = contexto.Cliente.AsNoTracking().Where(cliente => cliente.Uf == uf);

                if (somenteSemVinculo)
                {
                    query = query.Where(cliente => !clientesVinculados.Contains(cliente.IdCliente));
                }

                return query
                    .OrderBy(cliente => cliente.NomeCliente)
                    .Select(cliente => new VinculoOptionItem
                    {
                        Id = cliente.IdCliente,
                        Nome = cliente.NomeCliente,
                        Uf = cliente.Uf
                    })
                    .ToList();
            }
        }

        public IReadOnlyList<VinculoItem> ListarSistemasDisponiveis(string uf, int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                var vinculados = contexto.ClienteSistemas
                    .Where(item => item.IdCliente == idCliente)
                    .Select(item => item.IdSistemas);

                return contexto.Sistemas
                    .AsNoTracking()
                    .Where(sistema => sistema.Uf == uf && !vinculados.Contains(sistema.IdSistemas))
                    .OrderBy(sistema => sistema.NomeSistema)
                    .Select(sistema => new VinculoItem
                    {
                        Id = sistema.IdSistemas,
                        Nome = sistema.NomeSistema,
                        Uf = sistema.Uf
                    })
                    .ToList();
            }
        }

        public IReadOnlyList<VinculoItem> ListarSistemasVinculados(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                return (from clienteSistema in contexto.ClienteSistemas.AsNoTracking()
                        join sistema in contexto.Sistemas.AsNoTracking() on clienteSistema.IdSistemas equals sistema.IdSistemas
                        where clienteSistema.IdCliente == idCliente
                        orderby sistema.NomeSistema
                        select new VinculoItem
                        {
                            Id = sistema.IdSistemas,
                            Nome = sistema.NomeSistema,
                            Uf = sistema.Uf
                        }).ToList();
            }
        }

        public void Salvar(int idCliente, IEnumerable<int> sistemasVinculados)
        {
            using (var contexto = contextFactory.Create())
            {
                var idsDesejados = new HashSet<int>(sistemasVinculados);
                var vinculosAtuais = contexto.ClienteSistemas.Where(item => item.IdCliente == idCliente).ToList();
                var idsAtuais = new HashSet<int>(vinculosAtuais.Select(item => item.IdSistemas));

                foreach (var vinculo in vinculosAtuais.Where(item => !idsDesejados.Contains(item.IdSistemas)).ToList())
                {
                    contexto.ClienteSistemas.Remove(vinculo);
                }

                foreach (var idSistema in idsDesejados.Where(idSistema => !idsAtuais.Contains(idSistema)))
                {
                    contexto.ClienteSistemas.Add(new ClienteSistemas
                    {
                        IdCliente = idCliente,
                        IdSistemas = idSistema
                    });
                }

                contexto.SaveChanges();
            }
        }

        public void ExcluirVinculosDoCliente(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                var vinculos = contexto.ClienteSistemas.Where(item => item.IdCliente == idCliente).ToList();
                foreach (var vinculo in vinculos)
                {
                    contexto.ClienteSistemas.Remove(vinculo);
                }

                contexto.SaveChanges();
            }
        }
    }
}
