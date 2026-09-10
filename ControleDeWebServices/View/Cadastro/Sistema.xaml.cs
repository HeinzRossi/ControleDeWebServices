using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using ControleDeWebServices.View.Cadastro;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class Sistema : Page
    {
        private DadosDBContext contexto;

        public Acao Acao;
        public Sistemas idSistemas;
        private Frame FrameActive;

        public Sistema()
        {
            Acao = Acao.Inserir;
            InitializeComponent();            
            contexto = new DadosDBContext();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
        }
        public Sistema(Sistemas IdSistema)
        {
            Acao = Acao.Editar;
            idSistemas = IdSistema;
            InitializeComponent();
            contexto = new DadosDBContext();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
            BuscarRegistros();
        }
        private void BtnGravar_Click(object sender, RoutedEventArgs e)
        {
            PersistirDados(Acao);
            FrameActive.Content = new ListaDeSistemas();
        }
        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            FrameActive.Content = new ListaDeSistemas();
        }
        private void PersistirDados(Acao pAcao)
        {
            switch (pAcao)
            {
                case Acao.Inserir:
                    {
                        Sistemas Sistemas = new Sistemas()
                        {
                            NomeSistema = txtNomeDoSistema.Text,
                            Uf = cbbUF.Text
                        };

                        contexto.Sistemas.Add(Sistemas);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Editar:
                    {

                        var idServico = ReflectionDataGrid.CastObject<Sistemas>(idSistemas);

                        Sistemas Sistemas = contexto.Sistemas.Find(idServico.IdSistemas);
                        {
                            Sistemas.NomeSistema = txtNomeDoSistema.Text;
                            Sistemas.Uf = cbbUF.Text;
                        };

                        contexto.Entry(Sistemas);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Excluir:
                    {
                        var idServico = ReflectionDataGrid.CastObject<Sistemas>(idSistemas);

                        Sistemas Sistemas  = contexto.Sistemas.Find(idServico.IdSistemas);
                        contexto.Sistemas.Remove(Sistemas);
                        contexto.SaveChanges();
                        break;
                    }
            }
        }
        private void BuscarRegistros()
        {
            var idSistema = ReflectionDataGrid.CastObject<Sistemas>(idSistemas);

            Sistemas Sistemas = contexto.Sistemas.Find(idSistema.IdSistemas);
            {
                txtNomeDoSistema.Text = Sistemas.NomeSistema;
                cbbUF.SelectedValue = (Estados)Enum.Parse(typeof(Estados), idSistema.Uf.ToString());
            };

        }
        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
        }
    }
}
