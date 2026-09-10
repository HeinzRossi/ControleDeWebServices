using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class CadastroDeInformacoes : Page
    {
        public CadastroDeInformacoes(ConfiguracaoSistemaViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
