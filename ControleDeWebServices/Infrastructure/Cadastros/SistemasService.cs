using ControleDeWebServices.Application.Cadastros;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Cadastros
{
    public sealed class SistemasService : ISistemasService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public SistemasService(IDadosDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IReadOnlyList<SistemaListItem> Listar()
        {
            using (var contexto = contextFactory.Create())
            {
                return contexto.Sistemas
                    .AsNoTracking()
                    .OrderBy(sistema => sistema.NomeSistema)
                    .Select(sistema => new SistemaListItem
                    {
                        IdSistemas = sistema.IdSistemas,
                        NomeSistema = sistema.NomeSistema,
                        Uf = sistema.Uf
                    })
                    .ToList();
            }
        }

        public SistemaEditor ObterParaEdicao(int idSistemas)
        {
            using (var contexto = contextFactory.Create())
            {
                var sistema = contexto.Sistemas.AsNoTracking().FirstOrDefault(item => item.IdSistemas == idSistemas);
                if (sistema == null)
                {
                    throw new InvalidOperationException("Sistema nao encontrado.");
                }

                return new SistemaEditor
                {
                    IdSistemas = sistema.IdSistemas,
                    NomeSistema = sistema.NomeSistema,
                    Uf = sistema.Uf
                };
            }
        }

        public void Salvar(SistemaEditor editor)
        {
            using (var contexto = contextFactory.Create())
            {
                if (editor.IdSistemas.HasValue)
                {
                    var sistema = contexto.Sistemas.Find(editor.IdSistemas.Value);
                    if (sistema == null)
                    {
                        throw new InvalidOperationException("Sistema nao encontrado.");
                    }

                    sistema.NomeSistema = editor.NomeSistema;
                    sistema.Uf = editor.Uf;
                }
                else
                {
                    contexto.Sistemas.Add(new Sistemas
                    {
                        NomeSistema = editor.NomeSistema,
                        Uf = editor.Uf
                    });
                }

                contexto.SaveChanges();
            }
        }

        public void Excluir(int idSistemas)
        {
            using (var contexto = contextFactory.Create())
            {
                var sistema = contexto.Sistemas.Find(idSistemas);
                if (sistema == null)
                {
                    throw new InvalidOperationException("Sistema nao encontrado.");
                }

                contexto.Sistemas.Remove(sistema);
                contexto.SaveChanges();
            }
        }
    }
}
