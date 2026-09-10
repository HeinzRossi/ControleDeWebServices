using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Vinculos
{
    public partial class ListaVinculosServico : Page
    {
        public ListaVinculosServico(ListaVinculosServicoViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CarregarCommand.Execute(null);
        }
    }
}
