using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using ControleDeWebServices.View.Vinculos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class CadastroDeInformacoes : Page
    {
        private Frame FrameActive;
        private ClienteSistemas pClienteSistemas;
        private ObservableCollection<ParametrosSistema> parametrosSistemas;
        private List<ParametrosSistema> ListaParametros = new List<ParametrosSistema>();
        private DadosDBContext contexto = new DadosDBContext();

        public CadastroDeInformacoes(ClienteSistemas clienteSistemas)
        {
            InitializeComponent();
            cbbTipoConexao.ItemsSource = new List<TipoConexao>()
            {
                TipoConexao.FIREBIRD,
                TipoConexao.MSSQL,
                TipoConexao.SQLSERVER,
                TipoConexao.POSTGRESQL
            };
            cbbSecao.ItemsSource = BuscarSecoes();
            pClienteSistemas = clienteSistemas;
            PreencherCampos(pClienteSistemas);
            BuscarParametros(pClienteSistemas);

        }

        private IEnumerable BuscarSecoes()
        {
            return (from secao in contexto.Secao orderby secao.NomeSecao select secao).ToList();
        }

        private void BuscarParametros(ClienteSistemas pClienteSistemas)
        {
            parametrosSistemas = new ObservableCollection<ParametrosSistema>(from ParametrosSistema in contexto.ParametrosSistema
                                                                           where ParametrosSistema.IdClientesSistema == pClienteSistemas.IdClientesSistema
                                                                           select ParametrosSistema);
            GridParametros.ItemsSource = parametrosSistemas.ToList();

            foreach(var item in parametrosSistemas)
            {
                ListaParametros.Add(item);
            }
        }

        private void PreencherCampos(ClienteSistemas pClienteSistemas)
        {
            txtNomeUf.Text               = pClienteSistemas.Uf;
            txtNomeCliente.Text          = pClienteSistemas.NomeCliente;
            txtNomeDoArquivo.Text        = pClienteSistemas.ArquivoExecutavel;
            txtSistema.Text              = pClienteSistemas.NomeSistema;
            txtNomeDoCaminho.Text        = pClienteSistemas.ArquivoConfiguracao;
            txtServidor.Text             = pClienteSistemas.Servidor;
            txtPorta.Text                = pClienteSistemas.Porta.ToString();
            txtDataBase.Text             = pClienteSistemas.DataBase;
            cbbTipoConexao.SelectedValue = (TipoConexao)Enum.Parse(typeof(TipoConexao), pClienteSistemas.TipoConexao.ToString());
            txtUsuario.Text              = pClienteSistemas.Usuario;
            txtSenha.Password            = pClienteSistemas.Senha;
            cbbSecao.SelectedValue       = pClienteSistemas.IdSecao;
            chkCriptografia.IsChecked    = pClienteSistemas.UsaCriptografia;
        }

        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            FrameActive.Content = new ListaVinculosSistema();
        }

        private void TxtPorta_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascii = Convert.ToInt32(Convert.ToChar(e.Text));
            if (ascii >= 48 && ascii <= 57)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void BtnNomeCaminho_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            txtNomeDoCaminho.Text = dialog.FileName;
        }

        private void BtnGravar_Click(object sender, RoutedEventArgs e)
        {
            PersistirDados();
            Funcoes.AtivarDesativarControles(Botoes, false);
            Funcoes.AtivarDesativarControles(CamposDeServico, false);
            FrameActive.Content = new ListaVinculosSistema();
        }

        private void PersistirDados()
        {
            ClienteSistemas clienteSistemas = contexto.ClienteSistemas.Find(pClienteSistemas.IdClientesSistema);
            {
                clienteSistemas.ArquivoConfiguracao = txtNomeDoCaminho.Text;
                clienteSistemas.ArquivoExecutavel   = txtNomeDoArquivo.Text;
                clienteSistemas.Servidor            = txtServidor.Text;
                clienteSistemas.Porta               = txtPorta.Text.asIFElse();
                clienteSistemas.DataBase            = txtDataBase.Text;
                clienteSistemas.TipoConexao         = cbbTipoConexao.SelectedValue.asInt(); 
                clienteSistemas.Usuario             = txtUsuario.Text;
                clienteSistemas.Senha               = txtSenha.Password;
                clienteSistemas.IdSecao             = cbbSecao.SelectedValue.asInt();
                clienteSistemas.UsaCriptografia     = chkCriptografia.IsChecked;
            }

            foreach (ParametrosSistema item in ListaParametros)
            {
                switch (item.Status)
                {
                    case StatusServico.Inserir:
                        {
                            contexto.ParametrosSistema.Add(item);
                            break;
                        }
                    case StatusServico.Excluir:
                        {
                            ParametrosSistema parametrosSistema = contexto.ParametrosSistema.Where(
                                                                                                    p => p.IdClientesSistema == item.IdClientesSistema &&
                                                                                                    p.Secao == item.Secao &&
                                                                                                    p.Parametro == item.Parametro
                                                                                                   ).FirstOrDefault();
                            contexto.ParametrosSistema.Remove(parametrosSistema);
                            break;
                        }
                }
            }

            contexto.Entry(clienteSistemas);
            contexto.SaveChanges();
        }

        private void BtnNomearquivo_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            txtNomeDoArquivo.Text = dialog.FileName;
        }

        private void btnInserirParametro_Click(object sender, RoutedEventArgs e)
        {
            if ((txtSecao.Text.Trim() == "") || (txtParametro.Text.Trim() == ""))
            {
                System.Windows.MessageBox.Show("Favor Incluir o nome da SEÇÃO e o PARÂMETRO!","Atenção!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
                
            IncluirNaListaDeParametros();
        }

        private void IncluirNaListaDeParametros()
        {
            ParametrosSistema parametrosSistema = new ParametrosSistema();
            parametrosSistema.IdClientesSistema = pClienteSistemas.IdClientesSistema;
            parametrosSistema.Secao             = txtSecao.Text.Trim();
            parametrosSistema.Parametro         = txtParametro.Text.Trim();
            parametrosSistema.Valor             = txtValor.Text.Trim();
            parametrosSistema.Status            = StatusServico.Inserir;

            if (!ListaParametros.Any(p => p.Secao == parametrosSistema.Secao && p.Parametro == parametrosSistema.Parametro))
                ListaParametros.Add(parametrosSistema);

            GridParametros.ItemsSource = ListaParametros.ToList();
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button cmd = (System.Windows.Controls.Button)sender;
            if (cmd.DataContext is ParametrosSistema parametrosSistema)
            {
                parametrosSistema.Status = StatusServico.Excluir;
            }
            GridParametros.ItemsSource = ListaParametros.ToList().Where(p=> p.Status != StatusServico.Excluir);
        }

        private void btnImportarParametro_Click(object sender, RoutedEventArgs e)
        {
            
            ImportarParametrosDe importar = new ImportarParametrosDe(pClienteSistemas);
            importar.ShowDialog();
        }
    }
}
