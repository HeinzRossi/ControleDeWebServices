using ControleDeWebServices.ViewModels;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class ImportarParametrosDe : System.Windows.Window
    {
        public ImportarParametrosDe(ImportarParametrosViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.RequestClose += (sender, args) => Close();
        }
    }
}
