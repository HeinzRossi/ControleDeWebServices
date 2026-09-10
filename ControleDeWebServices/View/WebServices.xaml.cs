using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View
{
    public partial class WebServices : Page
    {
        enum TipoGrid { Grid10Mais,GridSistemas};
        private DadosDBContext contexto;
        public Cliente idCliente;
        private ObservableCollection<Cliente> ColecaoEstados;

        public WebServices()
        {
            contexto = new DadosDBContext();
            InitializeComponent();
            ListaDeEstados(cbbUF);
            BuscaMaisAcessados();
        }

        private void BuscaMaisAcessados()
        {
            var lista = (from sistema in contexto.Sistemas
                        join Clientesistema in contexto.ClienteSistemas on (sistema.IdSistemas) equals (Clientesistema.IdSistemas)
                        join cliente in contexto.Cliente on (Clientesistema.IdCliente) equals (cliente.IdCliente)
                        where (from clienteServicos in contexto.ClienteServicos
                               select clienteServicos.IdSistemas).Contains(sistema.IdSistemas)
                               orderby(Clientesistema.QuantidadeAcessos) descending
                         select new
                        {
                            Clientesistema.IdClientesSistema,
                            Clientesistema.IdCliente,
                            Clientesistema.IdSistemas,
                            cliente.CodigoControle,
                            sistema.NomeSistema,
                            cliente.NomeCliente,
                            cliente.Uf,
                            Clientesistema.DataBase,
                            Clientesistema.ArquivoConfiguracao,
                            Clientesistema.Porta,
                            Clientesistema.Senha,
                            Clientesistema.Servidor,
                            Clientesistema.TipoConexao,
                            Clientesistema.Usuario,
                            Clientesistema.QuantidadeAcessos,
                            Clientesistema.IdSecao,
                            Clientesistema.UsaCriptografia
                        }).Take(10);

            Grid10Mais.ItemsSource = lista.ToList();
        }

        private void ListaDeEstados(ComboBox listBox)
        {
            List<string> UFEstado = new List<string>();

            ColecaoEstados = new ObservableCollection<Cliente>(
                                                                from estados in contexto.Cliente
                                                                join clientesistemas in contexto.ClienteSistemas on (estados.IdCliente) equals (clientesistemas.IdCliente) into JG
                                                                from joineleft in JG.DefaultIfEmpty()
                                                                where (from clienteservicos in contexto.ClienteServicos
                                                                                          where clienteservicos.IdSistemas == joineleft.IdSistemas
                                                                                          select clienteservicos.IdCliente).Contains(estados.IdCliente)
                                                                select estados
                                                                );
            foreach (var lista in ColecaoEstados)
            {
                if (!UFEstado.Contains(lista.Uf))
                    UFEstado.Add(lista.Uf);
            };

            UFEstado.Sort();

            listBox.ItemsSource = UFEstado;
        }

        private void CbbUF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridSistemas.ItemsSource = null;

            BuscarRegistrosCliente(cbbUF.SelectedItem.ToString());
        }
        private void BuscarRegistrosCliente(string pEstado)
        {
            var lista = (from e in contexto.Cliente
                            join clientesistemas in contexto.ClienteSistemas on (e.IdCliente) equals (clientesistemas.IdCliente) into JG
                            from joineleft in JG.DefaultIfEmpty()
                            where e.Uf == pEstado && (from clienteservicos in contexto.ClienteServicos
                                                    where clienteservicos.IdSistemas == joineleft.IdSistemas
                                                    select clienteservicos.IdCliente).Contains(e.IdCliente)
                            select e).Distinct();
            cbbClientes.ItemsSource = lista.ToList();
        }
        private void CbbClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbClientes.SelectedItem == null)
                return;
            Cliente cliente = ReflectionDataGrid.CastObject<Cliente>(cbbClientes.SelectedItem);
            BuscarSistemas(cliente.IdCliente);
        }
        private void BuscarSistemas(int pIdCliente)
        {
            CarregarSistemas(pIdCliente);
        }
        private void CarregarSistemas(int pIdCliente)
        {
            var lista = from sistema in contexto.Sistemas
                        join Clientesistema in contexto.ClienteSistemas on (sistema.IdSistemas) equals (Clientesistema.IdSistemas)
                        join cliente in contexto.Cliente on (Clientesistema.IdCliente) equals (cliente.IdCliente)
                        where cliente.IdCliente == pIdCliente && (from clienteServicos in contexto.ClienteServicos
                                                                    select clienteServicos.IdSistemas).Contains(sistema.IdSistemas)
                        select new
                        {
                            Clientesistema.IdClientesSistema,
                            Clientesistema.IdCliente,
                            Clientesistema.IdSistemas,
                            sistema.NomeSistema,
                            cliente.CodigoControle,
                            cliente.NomeCliente,
                            cliente.Uf,
                            Clientesistema.DataBase,
                            Clientesistema.ArquivoConfiguracao,
                            Clientesistema.Porta,
                            Clientesistema.Senha,
                            Clientesistema.Servidor,
                            Clientesistema.TipoConexao,
                            Clientesistema.Usuario,
                            Clientesistema.QuantidadeAcessos,
                            Clientesistema.IdSecao,
                            Clientesistema.UsaCriptografia
                        };

            GridSistemas.ItemsSource = lista.ToList();
        }

        private void TxtIdCliente_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtIdCliente.Text != string.Empty)
                CarregarClientes(txtIdCliente.Text.asInt());
            else
                GridSistemas.ItemsSource = null;
        }

        private void CarregarClientes(int pcodigoControle)
        {
            var lista = from sistema in contexto.Sistemas
                        join Clientesistema in contexto.ClienteSistemas on (sistema.IdSistemas) equals (Clientesistema.IdSistemas)
                        join cliente in contexto.Cliente on (Clientesistema.IdCliente) equals (cliente.IdCliente)
                        where cliente.CodigoControle == pcodigoControle && (from clienteServicos in contexto.ClienteServicos
                                                                            select clienteServicos.IdSistemas).Contains(sistema.IdSistemas)
                        select new
                        {
                            Clientesistema.IdClientesSistema,
                            Clientesistema.IdCliente,
                            Clientesistema.IdSistemas,
                            sistema.NomeSistema,
                            cliente.CodigoControle,
                            cliente.NomeCliente,
                            cliente.Uf,
                            Clientesistema.DataBase,
                            Clientesistema.ArquivoConfiguracao,
                            Clientesistema.Porta,
                            Clientesistema.Senha,
                            Clientesistema.Servidor,
                            Clientesistema.TipoConexao,
                            Clientesistema.Usuario,
                            Clientesistema.QuantidadeAcessos,
                            Clientesistema.IdSecao,
                            Clientesistema.UsaCriptografia
                        };

            GridSistemas.ItemsSource = lista.ToList();
        }

        private void Grid10Mais_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var ItemGrid = ReflectionDataGrid.CastObject<ClienteSistemas>(Grid10Mais.SelectedItem);

            ExecutarSistemaServico executarSistemaServico = new ExecutarSistemaServico(ItemGrid);
            executarSistemaServico.ValidarConfiguracoesSistema()
                                  .ValidarConfiguracoesServico()
                                  .AtualizarArquivoConfiguracaoSistema()
                                  .AtualizarArquivoConfiguracaoServico()
                                  .AtualizarParametros()
                                  .DerrubarServicos()
                                  .ExecutarServicos()
                                  .AtualizarQuantidadeDeAcessos();
        }

        private void GridSistemas_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GridSistemas.SelectedItem == null)
                return;

            var ItemGrid = ReflectionDataGrid.CastObject<ClienteSistemas>(GridSistemas.SelectedItem);

            ExecutarSistemaServico executarSistemaServico = new ExecutarSistemaServico(ItemGrid);
            executarSistemaServico.ValidarConfiguracoesSistema()
                                  .ValidarConfiguracoesServico()
                                  .AtualizarArquivoConfiguracaoSistema()
                                  .AtualizarArquivoConfiguracaoServico()
                                  .AtualizarParametros()
                                  .DerrubarServicos()
                                  .ExecutarServicos()
                                  .AtualizarQuantidadeDeAcessos();
        }

        private void btnAtualizarUrlSistemas_Click(object sender, RoutedEventArgs e)
        {
            AtualizarUrl(TipoGrid.GridSistemas);
        }

        private void btnAtualizarUrlMais10_Click(object sender, RoutedEventArgs e)
        {
            AtualizarUrl(TipoGrid.Grid10Mais);
        }
        private void AtualizarUrl (TipoGrid tipoGrid)
        {
            AtualizarPadroes atualizarPadroes = new AtualizarPadroes();

            switch (tipoGrid)
            {
                case TipoGrid.Grid10Mais:
                    {
                        atualizarPadroes.AtualizarURL(ReflectionDataGrid.CastObject<ClienteSistemas>(Grid10Mais.SelectedItem));
                        break;
                    }
                case TipoGrid.GridSistemas:
                    {
                        atualizarPadroes.AtualizarURL(ReflectionDataGrid.CastObject<ClienteSistemas>(GridSistemas.SelectedItem));
                        break;
                    }
            }
        }
    }
}
