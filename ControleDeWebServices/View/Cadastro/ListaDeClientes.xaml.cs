using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeClientes : Page
    {
        private DadosDBContext contexto;
        public int idClientes;
        public ListaDeClientes()
        {
            InitializeComponent();
            contexto = new DadosDBContext();
            BuscarRegistros();


            TelaConsultaCliente.Visibility = Visibility.Visible;
            FrameInserirtEditar.Visibility = Visibility.Hidden;
        }
        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            TelaConsultaCliente.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var clientes = new Clientes();
            FrameInserirtEditar.Content = clientes;
            clientes.PassarContent(FrameInserirtEditar);
        }        
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var idCliente = ReflectionDataGrid.CastObject<Cliente>(GridClientes.SelectedItem);
            TelaConsultaCliente.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var clientes = new Clientes(idCliente);
            FrameInserirtEditar.Content = clientes;
            clientes.PassarContent(FrameInserirtEditar);
        }
        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Apagar Registro?", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var idCliente = ReflectionDataGrid.CastObject<Cliente>(GridClientes.SelectedItem);

                Cliente cliente = contexto.Cliente.Find(idCliente.IdCliente);
                contexto.Cliente.Remove(cliente);
                contexto.SaveChanges();
                BuscarRegistros();
            }
        }
        private void BuscarRegistros()
        {
            var lista = from e in contexto.Cliente
                        select new
                        {
                            e.IdCliente,
                            e.NomeCliente,
                            e.Uf
                        };
            GridClientes.SelectedIndex = 0;
            GridClientes.ItemsSource = lista.ToList();
            GridClientes.SelectedItem = lista.FirstOrDefault();
            foreach (DataGridColumn dataGridColumn in GridClientes.Columns) ;
        }
    }
}
