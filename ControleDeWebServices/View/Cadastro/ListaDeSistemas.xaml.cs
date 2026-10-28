using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeSistemas : Page
    {
        public ListaDeSistemas(SistemasListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CarregarCommand.Execute(null);
        }
    }
}
