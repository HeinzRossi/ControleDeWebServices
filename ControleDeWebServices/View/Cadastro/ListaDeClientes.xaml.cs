using ControleDeWebServices.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ListaDeClientes : Page
    {
        public ListaDeClientes()
        {
            InitializeComponent();

            var viewModel = App.Services.GetRequiredService<ClientesListViewModel>();
            DataContext = viewModel;
            viewModel.CarregarCommand.Execute(null);
        }
    }
}
