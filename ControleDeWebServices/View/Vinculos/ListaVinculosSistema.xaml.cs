using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using ControleDeWebServices.View.Cadastro;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Vinculos
{
    public partial class ListaVinculosSistema : Page
    {
        DadosDBContext contexto = new DadosDBContext();
        public ListaVinculosSistema()
        {
            InitializeComponent();
            ListarClientes();
            GridClientes.Focus();
        }

        private void ListarClientes()
        {
            var clientes = (from listacliente in contexto.Cliente
                            join clientesSistema in contexto.ClienteSistemas on (listacliente.IdCliente) equals (clientesSistema.IdCliente)
                            select listacliente).Distinct();

            GridClientes.ItemsSource = clientes.ToList();
            GridClientes.SelectedItem = clientes.FirstOrDefault();
        }

        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            TelaConsultaSistema.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            VinculoClienteSistema vinculoClienteSistema = new VinculoClienteSistema();
            FrameInserirtEditar.Content = vinculoClienteSistema;
            vinculoClienteSistema.PassarContent(FrameInserirtEditar);
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
            var sistemas = from listaDeSistemas in contexto.ClienteSistemas
                           join sistema in contexto.Sistemas on (listaDeSistemas.IdSistemas) equals (sistema.IdSistemas)
                           join cliente in contexto.Cliente on (idCliente) equals (cliente.IdCliente)
                           where listaDeSistemas.IdCliente == idCliente
                           select new 
                            {
                                listaDeSistemas.IdClientesSistema,
                                listaDeSistemas.IdCliente,
                                listaDeSistemas.IdSistemas,
                                cliente.NomeCliente,
                                sistema.NomeSistema,
                                sistema.Uf,
                                listaDeSistemas.ArquivoExecutavel,
                                listaDeSistemas.ArquivoConfiguracao,
                                listaDeSistemas.Servidor,
                                listaDeSistemas.Porta,
                                listaDeSistemas.DataBase,
                                listaDeSistemas.TipoConexao,
                                listaDeSistemas.Usuario,
                                listaDeSistemas.Senha,
                                listaDeSistemas.IdSecao,
                                listaDeSistemas.UsaCriptografia
                            };

            GridSistemas.ItemsSource = sistemas.ToList();
            GridSistemas.SelectedItem = sistemas.FirstOrDefault();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var idCliente = ReflectionDataGrid.CastObject<Cliente>(GridClientes.SelectedItem);

            TelaConsultaSistema.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;

            VinculoClienteSistema vinculoClienteSistema = new VinculoClienteSistema(idCliente);
            FrameInserirtEditar.Content = vinculoClienteSistema;
            vinculoClienteSistema.PassarContent(FrameInserirtEditar);
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Excluir Vinculos do Cliente?", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var idCliente = ReflectionDataGrid.CastObject<Cliente>(GridClientes.SelectedItem);



                var clienteSistemas =  contexto.ClienteSistemas.Where(X=> X.IdCliente == idCliente.IdCliente).ToList();

                foreach (ClienteSistemas item in clienteSistemas)
                    contexto.ClienteSistemas.Remove(item);

                contexto.SaveChanges();
                ListarClientes();
            }
        }

        private void BtnConfigurar_Click(object sender, RoutedEventArgs e)
        {
            var Informacoes = ReflectionDataGrid.CastObject<ClienteSistemas>(GridSistemas.SelectedItem);

            TelaConsultaSistema.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            CadastroDeInformacoes cadastroDeInformacoes = new CadastroDeInformacoes(Informacoes);
            FrameInserirtEditar.Content = cadastroDeInformacoes;
            cadastroDeInformacoes.PassarContent(FrameInserirtEditar);
        }
    }
}
