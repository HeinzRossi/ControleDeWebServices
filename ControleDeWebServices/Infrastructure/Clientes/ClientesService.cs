using ControleDeWebServices.Application.Clientes;
using ControleDeWebServices.Application.Data;
using ControleDeWebServices.Modelo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ControleDeWebServices.Infrastructure.Clientes
{
    public sealed class ClientesService : IClientesService
    {
        private readonly IDadosDbContextFactory contextFactory;

        public ClientesService(IDadosDbContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IReadOnlyList<ClienteListItem> Listar()
        {
            using (var contexto = contextFactory.Create())
            {
                return contexto.Cliente
                    .AsNoTracking()
                    .OrderBy(cliente => cliente.NomeCliente)
                    .Select(cliente => new ClienteListItem
                    {
                        IdCliente = cliente.IdCliente,
                        CodigoControle = cliente.CodigoControle,
                        NomeCliente = cliente.NomeCliente,
                        Uf = cliente.Uf
                    })
                    .ToList();
            }
        }

        public ClienteEditor ObterParaEdicao(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                var cliente = contexto.Cliente.AsNoTracking().FirstOrDefault(item => item.IdCliente == idCliente);
                if (cliente == null)
                {
                    throw new InvalidOperationException("Cliente nao encontrado.");
                }

                return new ClienteEditor
                {
                    IdCliente = cliente.IdCliente,
                    CodigoControle = cliente.CodigoControle,
                    NomeCliente = cliente.NomeCliente,
                    Uf = cliente.Uf
                };
            }
        }

        public void Salvar(ClienteEditor editor)
        {
            using (var contexto = contextFactory.Create())
            {
                if (editor.IdCliente.HasValue)
                {
                    Atualizar(contexto, editor);
                }
                else
                {
                    Inserir(contexto, editor);
                }

                contexto.SaveChanges();
            }
        }

        public void Excluir(int idCliente)
        {
            using (var contexto = contextFactory.Create())
            {
                var cliente = contexto.Cliente.Find(idCliente);
                if (cliente == null)
                {
                    throw new InvalidOperationException("Cliente nao encontrado.");
                }

                contexto.Cliente.Remove(cliente);
                contexto.SaveChanges();
            }
        }

        private static void Inserir(DadosDBContext contexto, ClienteEditor editor)
        {
            contexto.Cliente.Add(new Cliente
            {
                CodigoControle = editor.CodigoControle,
                NomeCliente = editor.NomeCliente,
                Uf = editor.Uf
            });
        }

        private static void Atualizar(DadosDBContext contexto, ClienteEditor editor)
        {
            var cliente = contexto.Cliente.Find(editor.IdCliente.Value);
            if (cliente == null)
            {
                throw new InvalidOperationException("Cliente nao encontrado.");
            }

            cliente.CodigoControle = editor.CodigoControle;
            cliente.NomeCliente = editor.NomeCliente;
            cliente.Uf = editor.Uf;
        }
    }
}
