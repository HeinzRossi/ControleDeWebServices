using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeSistemas : Page
    {
        private DadosDBContext contexto;
        public int idSistemas;
        public ListaDeSistemas()
        {
            InitializeComponent();
            contexto = new DadosDBContext();
            BuscarRegistros();


            TelaConsultaSistema.Visibility = Visibility.Visible;
            FrameInserirtEditar.Visibility = Visibility.Hidden;
        }

        private void BuscarRegistros()
        {
            var lista = from e in contexto.Sistemas
                        select new
                        {
                            e.IdSistemas,
                            e.NomeSistema,
                            e.Uf
                        };
            GridSistemas.SelectedIndex = 0;
            GridSistemas.ItemsSource = lista.ToList();
            GridSistemas.SelectedItem = lista.FirstOrDefault();
            foreach (DataGridColumn dataGridColumn in GridSistemas.Columns) ;
        }

        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            TelaConsultaSistema.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var sistema = new Sistema();
            FrameInserirtEditar.Content = sistema;
            sistema.PassarContent(FrameInserirtEditar);
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var idSistema = ReflectionDataGrid.CastObject<Sistemas>(GridSistemas.SelectedItem);
            TelaConsultaSistema.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var sistema = new Sistema(idSistema);
            FrameInserirtEditar.Content = sistema;
            sistema.PassarContent(FrameInserirtEditar);
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Apagar Registro?", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var idServico = ReflectionDataGrid.CastObject<Sistemas>(GridSistemas.SelectedItem);

                Sistemas Sistemas = contexto.Sistemas.Find(idServico.IdSistemas);
                contexto.Sistemas.Remove(Sistemas);
                contexto.SaveChanges();
                BuscarRegistros();
            }
        }

        private void GridSistemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
