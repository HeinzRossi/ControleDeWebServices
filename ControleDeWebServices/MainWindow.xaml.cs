using CommunityToolkit.Mvvm.Input;
using ControleDeWebServices.Application.Feedback;
using ControleDeWebServices.Presentation.Feedback;
using ControleDeWebServices.Presentation.Navigation;
using ControleDeWebServices.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControleDeWebServices
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel;
        private readonly WpfNavigationService navigationService;

        public ToastService ToastService { get; }
        public ConfirmDialogService ConfirmDialogService { get; }

        public MainWindow()
        {
            InitializeComponent();

            viewModel = App.Services.GetRequiredService<MainWindowViewModel>();
            navigationService = App.Services.GetRequiredService<WpfNavigationService>();
            ToastService = App.Services.GetRequiredService<ToastService>();
            ConfirmDialogService = App.Services.GetRequiredService<ConfirmDialogService>();

            DataContext = viewModel;
            navigationService.Attach(FramePrincipal);
            viewModel.ExitRequested += ViewModel_ExitRequested;

            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            EventManager.RegisterClassHandler(typeof(Button), Button.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            System.Windows.Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                ToastService.Show(new ToastRequest(ToastKind.Error, "Ocorreu um erro inesperado. Tente novamente ou acione o suporte.", "Atenção"));
                args.Handled = true;
            };
        }

        private void CadastrarServiços_Selected(object sender, RoutedEventArgs e)
        {
            ExecuteNavigation(viewModel.NavigateServicosCommand, e);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MoverProximo(e);
            }
        }

        private void MoverProximo(KeyEventArgs e)
        {
            var requisicao = new TraversalRequest(FocusNavigationDirection.Next);
            var controle = Keyboard.FocusedElement as UIElement;
            if (controle != null && controle.MoveFocus(requisicao))
            {
                e.Handled = true;
            }
        }

        private void CadastrarSistemas_Selected(object sender, RoutedEventArgs e)
        {
            ExecuteNavigation(viewModel.NavigateSistemasCommand, e);
        }

        private void Sair_Selected(object sender, RoutedEventArgs e)
        {
            viewModel.ExitCommand.Execute(null);
            e.Handled = true;
        }

        private void FormPrincipal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {
            }
        }

        private void FormPrincipal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void CadastrarClientes_Selected(object sender, RoutedEventArgs e)
        {
            ExecuteNavigation(viewModel.NavigateClientesCommand, e);
        }

        private void WebServices_Selected(object sender, RoutedEventArgs e)
        {
            ExecuteNavigation(viewModel.NavigateWebServicesCommand, e);
        }

        private void Cadastrar_Selected(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(sender, e.OriginalSource))
            {
                return;
            }

            Cadastrar.IsExpanded = !Cadastrar.IsExpanded;
            Cadastrar.IsSelected = false;
            e.Handled = true;
        }

        private void Vincular_Selected(object sender, RoutedEventArgs e)
        {
            if (!ReferenceEquals(sender, e.OriginalSource))
            {
                return;
            }

            Vincular.IsExpanded = !Vincular.IsExpanded;
            Vincular.IsSelected = false;
            e.Handled = true;
        }

        private void VincularSistemas_Selected(object sender, RoutedEventArgs e)
        {
            ExecuteNavigation(viewModel.NavigateVinculoClienteSistemaCommand, e);
        }

        private void VincularServicos_Selected(object sender, RoutedEventArgs e)
        {
            ExecuteNavigation(viewModel.NavigateVinculoClienteServicoCommand, e);
        }

        private void FormPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.NavigateWebServicesCommand.Execute(null);
        }

        private void CadastrarSecao_Selected(object sender, RoutedEventArgs e)
        {
            ExecuteNavigation(viewModel.NavigateSecoesCommand, e);
        }

        private void BtnToastFechar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ToastNotification toast)
            {
                ToastService.Dismiss(toast);
            }
        }

        private void BtnConfirmarDialog_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDialogService.Confirm();
        }

        private void BtnCancelarDialog_Click(object sender, RoutedEventArgs e)
        {
            ConfirmDialogService.Cancel();
        }

        private void ViewModel_ExitRequested(object sender, System.EventArgs e)
        {
            Close();
        }

        private static void ExecuteNavigation(IAsyncRelayCommand command, RoutedEventArgs e)
        {
            command.Execute(null);
            e.Handled = true;
        }
    }
}
