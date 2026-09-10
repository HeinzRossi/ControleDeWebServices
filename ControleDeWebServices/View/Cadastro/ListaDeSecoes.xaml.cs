using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeSecoes : Page
    {
        private DadosDBContext contexto;
        public int idSecao;
        public ListaDeSecoes()
        {
            InitializeComponent();
            contexto = new DadosDBContext();
            BuscarRegistros();


            TelaConsultaSecao.Visibility   = Visibility.Visible;
            FrameInserirtEditar.Visibility = Visibility.Hidden;
        }
        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            TelaConsultaSecao.Visibility   = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var secao = new Secoes();
            FrameInserirtEditar.Content = secao;
            secao.PassarContent(FrameInserirtEditar);
        }
        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var idSecao = ReflectionDataGrid.CastObject<Secao>(GridSecao.SelectedItem);
            TelaConsultaSecao.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var secao = new Secoes(idSecao);
            FrameInserirtEditar.Content = secao;
            secao.PassarContent(FrameInserirtEditar);
        }
        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Apagar Registro?", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var idSecao = ReflectionDataGrid.CastObject<Secao>(GridSecao.SelectedItem);

                Secao secao = contexto.Secao.Find(idSecao.IdSecao);
                contexto.Secao.Remove(secao);
                contexto.SaveChanges();
                BuscarRegistros();
            }
        }
        private void BuscarRegistros()
        {
            var lista = from e in contexto.Secao
                        select new
                        {
                            e.IdSecao,
                            e.NomeSecao,
                        };
            GridSecao.SelectedIndex = 0;
            GridSecao.ItemsSource = lista.ToList();
            GridSecao.SelectedItem = lista.FirstOrDefault();
            foreach (DataGridColumn dataGridColumn in GridSecao.Columns) ;
        }

    }
}
