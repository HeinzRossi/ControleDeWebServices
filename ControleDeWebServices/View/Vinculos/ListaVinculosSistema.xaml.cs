using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Vinculos
{
    public partial class ListaVinculosSistema : Page
    {
        public ListaVinculosSistema(ListaVinculosSistemaViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CarregarCommand.Execute(null);
        }
    }
}
