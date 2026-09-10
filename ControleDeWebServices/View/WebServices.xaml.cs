using ControleDeWebServices.ViewModels;
using System.Windows.Controls;

namespace ControleDeWebServices.View
{
    public partial class WebServices : Page
    {
        public WebServices(WebServicesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.CarregarCommand.Execute(null);
        }
    }
}
