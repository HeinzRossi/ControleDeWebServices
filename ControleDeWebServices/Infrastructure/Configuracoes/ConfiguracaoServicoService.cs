using ControleDeWebServices.Application.Configuracoes;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Diversos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Configuracoes
{
    public sealed class ConfiguracaoServicoService : IConfiguracaoServicoService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public ConfiguracaoServicoService(IDadosDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public ConfiguracaoServicoEditor Obter(int idClienteServico)
        {
            using (var contexto = contextFactory.Create())
            {
                var editor = (from clienteServico in contexto.ClienteServicos.AsNoTracking()
                              join cliente in contexto.Cliente.AsNoTracking() on clienteServico.IdCliente equals cliente.IdCliente
                              join sistema in contexto.Sistemas.AsNoTracking() on clienteServico.IdSistemas equals sistema.IdSistemas
                              join servico in contexto.Servicos.AsNoTracking() on clienteServico.IdServicos equals servico.IdServicos
                              where clienteServico.IdClienteServico == idClienteServico
                              select new ConfiguracaoServicoEditor
                              {
                                  IdClienteServico = clienteServico.IdClienteServico,
                                  IdCliente = clienteServico.IdCliente,
                                  IdSistemas = clienteServico.IdSistemas,
                                  IdServicos = clienteServico.IdServicos,
                                  Uf = cliente.Uf,
                                  NomeCliente = cliente.NomeCliente,
                                  NomeSistema = sistema.NomeSistema,
                                  NomeServico = servico.NomeServico,
                                  ArquivoExecutavel = clienteServico.ArquivoExecutavel,
                                  ArquivoConfiguracao = clienteServico.ArquivoConfiguracao,
                                  Servidor = clienteServico.Servidor,
                                  Porta = clienteServico.Porta,
                                  DataBase = clienteServico.DataBase,
                                  TipoConexao = clienteServico.TipoConexao,
                                  Usuario = clienteServico.Usuario,
                                  Senha = clienteServico.Senha
                              }).FirstOrDefault();

                if (editor == null)
                {
                    throw new InvalidOperationException("Configuração do serviço não encontrada.");
                }

                return editor;
            }
        }

        public IReadOnlyList<TipoConexaoOption> ListarTiposConexao()
        {
            return new[]
            {
                TipoConexao.FIREBIRD,
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

        public void Salvar(ConfiguracaoServicoEditor editor)
        {
            using (var contexto = contextFactory.Create())
            {
                var clienteServico = contexto.ClienteServicos.Find(editor.IdClienteServico);
                if (clienteServico == null)
                {
                    throw new InvalidOperationException("Configuração do serviço não encontrada.");
                }

                clienteServico.ArquivoExecutavel = editor.ArquivoExecutavel;
                clienteServico.ArquivoConfiguracao = editor.ArquivoConfiguracao;
                clienteServico.Servidor = editor.Servidor;
                clienteServico.Porta = editor.Porta;
                clienteServico.DataBase = editor.DataBase;
                clienteServico.TipoConexao = editor.TipoConexao;
                clienteServico.Usuario = editor.Usuario;
                clienteServico.Senha = editor.Senha;
                clienteServico.Uf = editor.Uf;

                contexto.SaveChanges();
            }
        }
    }
}
