using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using ControleDeWebServices.View.Cadastro;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Vinculos
{
    public partial class ListaVinculosServico : Page
    {
        DadosDBContext contexto = new DadosDBContext();
        public ListaVinculosServico()
        {
            InitializeComponent();
            ListarClientes();
        }

        private void ListarClientes()
        {
            var clientes = (from listacliente in contexto.Cliente
                            join clientesServico in contexto.ClienteServicos on (listacliente.IdCliente) equals (clientesServico.IdCliente)
                            select listacliente).Distinct();

            GridClientes.ItemsSource = clientes.ToList();
            GridClientes.SelectedItem = clientes.FirstOrDefault();
        }

        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            TelaConsultaServico.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            VinculoClienteServico vinculoClienteServico = new VinculoClienteServico();
            FrameInserirtEditar.Content = vinculoClienteServico;
            vinculoClienteServico.PassarContent(FrameInserirtEditar);
        }

        private void GridClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridClientes.SelectedItem == null)
                return;

            var cliente = ReflectionDataGrid.CastObject<Cliente>(GridClientes.SelectedItem);
            BuscarSistemas(cliente.IdCliente);
        }
        private void BuscarSistemas(int idCliente)
        {
            var sistemas = (from listaDeServicos in contexto.ClienteServicos
                           join sistema in contexto.Sistemas on (listaDeServicos.IdSistemas) equals (sistema.IdSistemas)
                           join cliente in contexto.Cliente on (idCliente) equals (cliente.IdCliente)
                           where listaDeServicos.IdCliente == idCliente
                           select new
                           {
                               listaDeServicos.IdCliente,
                               listaDeServicos.IdSistemas,
                               cliente.NomeCliente,
                               sistema.NomeSistema,
                               sistema.Uf
                           }).Distinct();

            GridSistemas.ItemsSource = sistemas.ToList();
            GridSistemas.SelectedItem = sistemas.FirstOrDefault();
        }

        private void GridSistemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridSistemas.SelectedItem == null)
                return;

            var sistema = ReflectionDataGrid.CastObject<ClienteSistemas>(GridSistemas.SelectedItem);
            BuscarServicos(sistema);
        }

        private void BuscarServicos(ClienteSistemas pClienteSistemas)
        {
            var servicos = from listaDeServicos in contexto.ClienteServicos
                           join cliente in contexto.Cliente on listaDeServicos.IdCliente equals cliente.IdCliente
                           join sistema in contexto.Sistemas on (listaDeServicos.IdSistemas) equals (sistema.IdSistemas)
                           join servico in contexto.Servicos on listaDeServicos.IdServicos equals servico.IdServicos
                           where listaDeServicos.IdCliente == pClienteSistemas.IdCliente && listaDeServicos.IdSistemas == pClienteSistemas.IdSistemas
                           select new
                           {
                               listaDeServicos.IdClienteServico,
                               listaDeServicos.IdCliente,
                               listaDeServicos.IdSistemas,
                               cliente.NomeCliente,
                               sistema.NomeSistema, 
                               servico.NomeServico,
                               cliente.Uf,
                               listaDeServicos.ArquivoExecutavel,
                               listaDeServicos.ArquivoConfiguracao,
                               listaDeServicos.Servidor,
                               listaDeServicos.Porta,
                               listaDeServicos.DataBase,
                               listaDeServicos.TipoConexao,
                               listaDeServicos.Usuario,
                               listaDeServicos.Senha
                           };

            GridServicos.ItemsSource = servicos.ToList();
            GridServicos.SelectedItem = servicos.FirstOrDefault();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var idCliente = ReflectionDataGrid.CastObject<ClienteSistemas>(GridSistemas.SelectedItem);

            TelaConsultaServico.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;

            VinculoClienteServico vinculoClienteServico = new VinculoClienteServico(idCliente);
            FrameInserirtEditar.Content = vinculoClienteServico;
            vinculoClienteServico.PassarContent(FrameInserirtEditar);
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Excluir Vinculos do Sistema?", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var idClienteSistema = ReflectionDataGrid.CastObject<ClienteSistemas>(GridSistemas.SelectedItem);



                var clienteServicos = contexto.ClienteServicos.Where(X => X.IdCliente == idClienteSistema.IdCliente && X.IdSistemas == idClienteSistema.IdSistemas).ToList();

                foreach (ClienteServicos item in clienteServicos)
                    contexto.ClienteServicos.Remove(item);

                contexto.SaveChanges();
                BuscarSistemas(idClienteSistema.IdCliente);
            }
        }

        private void BtnConfigurar_Click(object sender, RoutedEventArgs e)
        {
            var Informacoes = ReflectionDataGrid.CastObject<ClienteServicos>(GridServicos.SelectedItem);

            TelaConsultaServico.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            CadastroDeInformacoesServico cadastroDeInformacoesServico = new CadastroDeInformacoesServico(Informacoes);
            FrameInserirtEditar.Content = cadastroDeInformacoesServico;
            cadastroDeInformacoesServico.PassarContent(FrameInserirtEditar);
        }
    }
}
