using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Cadastros
{
    public sealed class SecoesService : ISecoesService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public SecoesService(IDadosDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IReadOnlyList<SecaoListItem> Listar()
        {
            using (var contexto = contextFactory.Create())
            {
                return contexto.Secao
                    .AsNoTracking()
                    .OrderBy(secao => secao.NomeSecao)
                    .Select(secao => new SecaoListItem
                    {
                        IdSecao = secao.IdSecao,
                        NomeSecao = secao.NomeSecao
                    })
                    .ToList();
            }
        }

        public SecaoEditor ObterParaEdicao(int idSecao)
        {
            using (var contexto = contextFactory.Create())
            {
                var secao = contexto.Secao.AsNoTracking().FirstOrDefault(item => item.IdSecao == idSecao);
                if (secao == null)
                {
                    throw new InvalidOperationException("Secao nao encontrada.");
                }

                return new SecaoEditor
                {
                    IdSecao = secao.IdSecao,
                    NomeSecao = secao.NomeSecao
                };
            }
        }

        public void Salvar(SecaoEditor editor)
        {
            using (var contexto = contextFactory.Create())
            {
                if (editor.IdSecao.HasValue)
                {
                    var secao = contexto.Secao.Find(editor.IdSecao.Value);
                    if (secao == null)
                    {
                        throw new InvalidOperationException("Secao nao encontrada.");
                    }

                    secao.NomeSecao = editor.NomeSecao;
                }
                else
                {
                    contexto.Secao.Add(new Secao
                    {
                        NomeSecao = editor.NomeSecao
                    });
                }

                contexto.SaveChanges();
            }
        }

        public void Excluir(int idSecao)
        {
            using (var contexto = contextFactory.Create())
            {
                var secao = contexto.Secao.Find(idSecao);
                if (secao == null)
                {
                    throw new InvalidOperationException("Secao nao encontrada.");
                }

                contexto.Secao.Remove(secao);
                contexto.SaveChanges();
            }
        }
    }
}
