using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeSecoes : Page
    {
        public ListaDeSecoes(SecoesListViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.CarregarCommand.Execute(null);
        }
    }
}
