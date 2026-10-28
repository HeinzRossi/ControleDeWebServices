using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Cadastros
{
    public sealed class ServicosService : IServicosService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public ServicosService(IDadosDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IReadOnlyList<ServicoListItem> Listar()
        {
            using (var contexto = contextFactory.Create())
            {
                return contexto.Servicos
                    .AsNoTracking()
                    .OrderBy(servico => servico.NomeServico)
                    .Select(servico => new ServicoListItem
                    {
                        IdServicos = servico.IdServicos,
                        NomeServico = servico.NomeServico,
                        Uf = servico.Uf
                    })
                    .ToList();
            }
        }

        public ServicoEditor ObterParaEdicao(int idServicos)
        {
            using (var contexto = contextFactory.Create())
            {
                var servico = contexto.Servicos.AsNoTracking().FirstOrDefault(item => item.IdServicos == idServicos);
                if (servico == null)
                {
                    throw new InvalidOperationException("Servico nao encontrado.");
                }

                return new ServicoEditor
                {
                    IdServicos = servico.IdServicos,
                    NomeServico = servico.NomeServico,
                    Uf = servico.Uf
                };
            }
        }

        public void Salvar(ServicoEditor editor)
        {
            using (var contexto = contextFactory.Create())
            {
                if (editor.IdServicos.HasValue)
                {
                    var servico = contexto.Servicos.Find(editor.IdServicos.Value);
                    if (servico == null)
                    {
                        throw new InvalidOperationException("Servico nao encontrado.");
                    }

                    servico.NomeServico = editor.NomeServico;
                    servico.Uf = editor.Uf;
                }
                else
                {
                    contexto.Servicos.Add(new Servicos
                    {
                        NomeServico = editor.NomeServico,
                        Uf = editor.Uf
                    });
                }

                contexto.SaveChanges();
            }
        }

        public void Excluir(int idServicos)
        {
            using (var contexto = contextFactory.Create())
            {
                var servico = contexto.Servicos.Find(idServicos);
                if (servico == null)
                {
                    throw new InvalidOperationException("Servico nao encontrado.");
                }

                contexto.Servicos.Remove(servico);
                contexto.SaveChanges();
            }
        }
    }
}
