using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Configuracoes
{
    public sealed class ConfiguracaoSistemaService : IConfiguracaoSistemaService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public ConfiguracaoSistemaService(IDadosDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public ConfiguracaoSistemaEditor Obter(int idClientesSistema)
        {
            using (var contexto = contextFactory.Create())
            {
                var editor = (from clienteSistema in contexto.ClienteSistemas.AsNoTracking()
                              join cliente in contexto.Cliente.AsNoTracking() on clienteSistema.IdCliente equals cliente.IdCliente
                              join sistema in contexto.Sistemas.AsNoTracking() on clienteSistema.IdSistemas equals sistema.IdSistemas
                              where clienteSistema.IdClientesSistema == idClientesSistema
                              select new ConfiguracaoSistemaEditor
                              {
                                  IdClientesSistema = clienteSistema.IdClientesSistema,
                                  IdCliente = clienteSistema.IdCliente,
                                  IdSistemas = clienteSistema.IdSistemas,
                                  Uf = cliente.Uf,
                                  NomeCliente = cliente.NomeCliente,
                                  NomeSistema = sistema.NomeSistema,
                                  ArquivoExecutavel = clienteSistema.ArquivoExecutavel,
                                  ArquivoConfiguracao = clienteSistema.ArquivoConfiguracao,
                                  Servidor = clienteSistema.Servidor,
                                  Porta = clienteSistema.Porta,
                                  DataBase = clienteSistema.DataBase,
                                  TipoConexao = clienteSistema.TipoConexao,
                                  Usuario = clienteSistema.Usuario,
                                  Senha = clienteSistema.Senha,
                                  IdSecao = clienteSistema.IdSecao,
                                  UsaCriptografia = clienteSistema.UsaCriptografia == true
                              }).FirstOrDefault();

                if (editor == null)
                {
                    throw new InvalidOperationException("Configuração do sistema não encontrada.");
                }

                editor.Parametros = contexto.ParametrosSistema
                    .AsNoTracking()
                    .Where(parametro => parametro.IdClientesSistema == idClientesSistema)
                    .OrderBy(parametro => parametro.Secao)
                    .ThenBy(parametro => parametro.Parametro)
                    .Select(parametro => new ParametroEditor
                    {
                        IdParametrosSistema = parametro.IdParametrosSistema,
                        IdClientesSistema = parametro.IdClientesSistema,
                        Secao = parametro.Secao,
                        Parametro = parametro.Parametro,
                        Valor = parametro.Valor
                    })
                    .ToList();

                return editor;
            }
        }

        public IReadOnlyList<TipoConexaoOption> ListarTiposConexao()
        {
            return new[]
            {
                TipoConexao.FIREBIRD,
                TipoConexao.MSSQL,
                TipoConexao.SQLSERVER,
                TipoConexao.POSTGRESQL
            }
            .Select(tipo => new TipoConexaoOption
            {
                Id = (int)tipo,
                Nome = tipo.ToString()
            })
            .ToList();
        }

        public IReadOnlyList<SecaoOption> ListarSecoes()
        {
            using (var contexto = contextFactory.Create())
            {
                return contexto.Secao
                    .AsNoTracking()
                    .OrderBy(secao => secao.NomeSecao)
                    .Select(secao => new SecaoOption
                    {
                        IdSecao = secao.IdSecao,
                        NomeSecao = secao.NomeSecao
                    })
                    .ToList();
            }
        }

        public void Salvar(ConfiguracaoSistemaEditor editor)
        {
            using (var contexto = contextFactory.Create())
            {
                var clienteSistema = contexto.ClienteSistemas.Find(editor.IdClientesSistema);
                if (clienteSistema == null)
                {
                    throw new InvalidOperationException("Configuração do sistema não encontrada.");
                }

                clienteSistema.ArquivoExecutavel = editor.ArquivoExecutavel;
                clienteSistema.ArquivoConfiguracao = editor.ArquivoConfiguracao;
                clienteSistema.Servidor = editor.Servidor;
                clienteSistema.Porta = editor.Porta;
                clienteSistema.DataBase = editor.DataBase;
                clienteSistema.TipoConexao = editor.TipoConexao;
                clienteSistema.Usuario = editor.Usuario;
                clienteSistema.Senha = editor.Senha;
                clienteSistema.IdSecao = editor.IdSecao;
                clienteSistema.UsaCriptografia = editor.UsaCriptografia;

                var parametrosDesejados = editor.Parametros ?? Array.Empty<ParametroEditor>();
                var parametrosAtuais = contexto.ParametrosSistema
                    .Where(parametro => parametro.IdClientesSistema == editor.IdClientesSistema)
                    .ToList();
                var idsDesejados = new HashSet<int>(parametrosDesejados
                    .Where(parametro => parametro.IdParametrosSistema > 0)
                    .Select(parametro => parametro.IdParametrosSistema));

                foreach (var parametroAtual in parametrosAtuais.Where(parametro => !idsDesejados.Contains(parametro.IdParametrosSistema)).ToList())
                {
                    contexto.ParametrosSistema.Remove(parametroAtual);
                }

                foreach (var parametroEditor in parametrosDesejados)
                {
                    var parametro = parametroEditor.IdParametrosSistema > 0
                        ? parametrosAtuais.FirstOrDefault(item => item.IdParametrosSistema == parametroEditor.IdParametrosSistema)
                        : null;

                    if (parametro == null)
                    {
                        parametro = new ParametrosSistema
                        {
                            IdClientesSistema = editor.IdClientesSistema
                        };
                        contexto.ParametrosSistema.Add(parametro);
                    }

                    parametro.Secao = parametroEditor.Secao;
                    parametro.Parametro = parametroEditor.Parametro;
                    parametro.Valor = parametroEditor.Valor;
                }

                contexto.SaveChanges();
            }
        }
    }
}
