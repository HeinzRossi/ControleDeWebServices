using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class CadastroDeInformacoesServico : Page
    {
        public CadastroDeInformacoesServico(ConfiguracaoServicoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
