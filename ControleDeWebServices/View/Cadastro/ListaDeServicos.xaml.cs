using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeServicos : Page
    {
        private DadosDBContext contexto;
        public int idServicos;
        public ListaDeServicos()
        {
            InitializeComponent();
            contexto = new DadosDBContext();
            BuscarRegistros();


            TelaConsultaServicos.Visibility = Visibility.Visible;
            FrameInserirtEditar.Visibility = Visibility.Hidden;
        }

        private void BuscarRegistros()
        {
            var lista = from e in contexto.Servicos
                        select new
                        {
                            e.IdServicos,
                            e.NomeServico,
                            e.Uf
                        };
            GridServicos.SelectedIndex = 0;
            GridServicos.ItemsSource = lista.ToList();
            GridServicos.SelectedItem = lista.FirstOrDefault();
            foreach (DataGridColumn dataGridColumn in GridServicos.Columns) ;
        }

        private void BtnIncluir_Click(object sender, RoutedEventArgs e)
        {
            TelaConsultaServicos.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var servico = new Servico();
            FrameInserirtEditar.Content = servico;
            servico.PassarContent(FrameInserirtEditar);

        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var idServico = ReflectionDataGrid.CastObject<Servicos>(GridServicos.SelectedItem);
            TelaConsultaServicos.Visibility = Visibility.Hidden;
            FrameInserirtEditar.Visibility = Visibility.Visible;
            var servico = new Servico(idServico);
            FrameInserirtEditar.Content = servico;
            servico.PassarContent(FrameInserirtEditar);
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Apagar Registro?", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                var idServico = ReflectionDataGrid.CastObject<Servicos>(GridServicos.SelectedItem);

                Servicos servicos = contexto.Servicos.Find(idServico.IdServicos);
                contexto.Servicos.Remove(servicos);
                contexto.SaveChanges();
                BuscarRegistros();
            }
        }
    }
}
