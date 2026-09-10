using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class Servico : Page
    {
        private DadosDBContext contexto;
        public Servicos idServicos;
        private Frame FrameActive;

        public Acao Acao;

        public Servico()
        {
            Acao = Acao.Inserir;
            InitializeComponent();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
            contexto = new DadosDBContext();
        }
        public Servico(Servicos idServico)
        {
            Acao = Acao.Editar;
            idServicos = idServico;
            InitializeComponent();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
            contexto = new DadosDBContext();
            BuscarRegistros();
        }

        private void BuscarRegistros()
        {
            var idSistema = ReflectionDataGrid.CastObject<Servicos>(idServicos);

            Servicos Sistemas = contexto.Servicos.Find(idSistema.IdServicos);
            {
                txtNomeDoServico.Text = idSistema.NomeServico;
                cbbUF.SelectedValue = (Estados)Enum.Parse(typeof(Estados), idSistema.Uf.ToString());
            };
        }

        private void BtnGravar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PersistirDados(Acao);
            FrameActive.Content = new ListaDeServicos();
        }
        private void BtnCancelar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FrameActive.Content = new ListaDeServicos();
        }
        private void PersistirDados(Acao pAcao)
        {
            switch (pAcao)
            {
                case Acao.Inserir:
                    {
                        Servicos servicos = new Servicos()
                        {
                            NomeServico = txtNomeDoServico.Text,
                            Uf = cbbUF.Text
                        };
                        contexto.Servicos.Add(servicos);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Editar:
                    {

                        var idServico = ReflectionDataGrid.CastObject<Servicos>(idServicos);

                        Servicos servicos = contexto.Servicos.Find(idServico.IdServicos);
                        {
                            servicos.NomeServico = txtNomeDoServico.Text;
                            servicos.Uf = cbbUF.Text;
                        };
                        contexto.Entry(servicos);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Excluir:
                    {
                        var idServico = ReflectionDataGrid.CastObject<Servicos>(idServicos);

                        Servicos servicos = contexto.Servicos.Find(idServico.IdServicos);

                        contexto.Servicos.Remove(servicos);
                        contexto.SaveChanges();
                        break;
                    }
            }
        }
        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
        }
    }
}
