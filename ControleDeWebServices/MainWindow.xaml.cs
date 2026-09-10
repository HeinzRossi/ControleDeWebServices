using ControleDeWebServices.View;
using ControleDeWebServices.View.Cadastro;
using ControleDeWebServices.View.Vinculos;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ControleDeWebServices
{
    public partial class MainWindow : Window
    {
        public DadosDBContext context;
        public MainWindow()
        {
            InitializeComponent();
            context = new DadosDBContext();
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            EventManager.RegisterClassHandler(typeof(Button), Button.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            EventManager.RegisterClassHandler(typeof(ComboBox), ComboBox.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            Application.Current.DispatcherUnhandledException += (sender, args) =>
            {
                MessageBox.Show(args.Exception.Message, "Atenção!", MessageBoxButton.OK, MessageBoxImage.Information);
                args.Handled = true;
            };
        }

        private void CadastrarServiços_Selected(object sender, RoutedEventArgs e)
        {
            ListaDeServicos listaDeServicos = new ListaDeServicos();
            this.FramePrincipal.Content = listaDeServicos;

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
            var controle = (Keyboard.FocusedElement as UIElement);
            if (controle != null && controle.MoveFocus(requisicao))
            {
                e.Handled = true;
            }
        }

        private void CadastrarSistemas_Selected(object sender, RoutedEventArgs e)
        {
            ListaDeSistemas listaDeSistemas = new ListaDeSistemas();
            this.FramePrincipal.Content = listaDeSistemas;
        }

        private void Sair_Selected(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void FormPrincipal_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
            ;
        }

        private void FormPrincipal_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void CadastrarClientes_Selected(object sender, RoutedEventArgs e)
        {
            ListaDeClientes listaDeClientes = new ListaDeClientes();
            this.FramePrincipal.Content = listaDeClientes;
        }

        private void WebServices_Selected(object sender, RoutedEventArgs e)
        {
            WebServices webServices = new WebServices();
            this.FramePrincipal.Content = webServices;
        }

        private void Cadastrar_Selected(object sender, RoutedEventArgs e)
        {
            if (Cadastrar.IsExpanded)
            {
                Cadastrar.IsExpanded = false;
            }
            else
            {
                Cadastrar.IsExpanded = true;
            }
            Cadastrar.IsSelected = false;
        }

        private void Vincular_Selected(object sender, RoutedEventArgs e)
        {
            if (Vincular.IsExpanded)
            {
                Vincular.IsExpanded = false;
            }
            else
            {
                Vincular.IsExpanded = true;
            }
            Vincular.IsSelected = false;
        }

        private void VincularSistemas_Selected(object sender, RoutedEventArgs e)
        {
            ListaVinculosSistema listaVinculosSistema = new ListaVinculosSistema();
            this.FramePrincipal.Content = listaVinculosSistema;
        }

        private void VincularServicos_Selected(object sender, RoutedEventArgs e)
        {
            ListaVinculosServico listaVinculoServico = new ListaVinculosServico();
            this.FramePrincipal.Content = listaVinculoServico;
        }

        private void FormPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            WebServices webServices = new WebServices();
            this.FramePrincipal.Content = webServices;
        }

        private void CadastrarSecao_Selected(object sender, RoutedEventArgs e)
        {
            ListaDeSecoes listaDeSecoes = new ListaDeSecoes();
            this.FramePrincipal.Content = listaDeSecoes;
        }
    }
}
