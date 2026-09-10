using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class Secoes : Page
    {
        private DadosDBContext contexto;
        public Acao Acao;
        public Secao idSecao;
        private Frame FrameActive;

        public Secoes()
        {
            Acao = Acao.Inserir;
            InitializeComponent();
            contexto = new DadosDBContext();
        }
        public Secoes(Secao IdSecao)
        {
            Acao = Acao.Editar;
            idSecao = IdSecao;
            InitializeComponent();
            contexto = new DadosDBContext();
            BuscarRegistros();
        }
        private void BtnGravar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCampos())
                return;
            PersistirDados(Acao);
            FrameActive.Content = new ListaDeSecoes();
        }
        private bool ValidarCampos()
        {
            if ((txtNomeSecao.Text.ToString() == null) || (txtNomeSecao.Text.ToString() == ""))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Favor informar o nome da Seção!", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    txtNomeSecao.Focus();
                    return false;
                }
            }
            return true;
        }
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            FrameActive.Content = new ListaDeSecoes();
        }
        private void PersistirDados(Acao pAcao)
        {
            switch (pAcao)
            {
                case Acao.Inserir:
                    {
                        Secao secao = new Secao()
                        {
                            NomeSecao = txtNomeSecao.Text,
                        };

                        contexto.Secao.Add(secao);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Editar:
                    {

                        var idSecoes = ReflectionDataGrid.CastObject<Secao>(this.idSecao);

                        Secao secao = contexto.Secao.Find(idSecoes.IdSecao);
                        {
                            secao.NomeSecao = txtNomeSecao.Text;
                        };

                        contexto.Entry(secao);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Excluir:
                    {
                        var idSecoes = ReflectionDataGrid.CastObject<Secao>(idSecao);

                        Sistemas Sistemas = contexto.Sistemas.Find(idSecoes.IdSecao);
                        contexto.Sistemas.Remove(Sistemas);
                        contexto.SaveChanges();
                        break;
                    }
            }
        }
        private void BuscarRegistros()
        {
            var idSecao = ReflectionDataGrid.CastObject<Secao>(this.idSecao);

            Secao secao = contexto.Secao.Find(idSecao.IdSecao);
            {
                txtNomeSecao.Text = secao.NomeSecao;
            };
        }
        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
        }

    }
}
