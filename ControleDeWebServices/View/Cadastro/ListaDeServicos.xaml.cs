using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeServicos : Page
    {
        public ListaDeServicos(ServicosListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CarregarCommand.Execute(null);
        }
    }
}
