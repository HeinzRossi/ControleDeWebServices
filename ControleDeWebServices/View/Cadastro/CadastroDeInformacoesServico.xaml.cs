using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using ControleDeWebServices.View.Vinculos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class CadastroDeInformacoesServico : Page
    {
        private Frame FrameActive;
        private ClienteServicos pClienteServicos;
        private DadosDBContext contexto = new DadosDBContext();
        public CadastroDeInformacoesServico(ClienteServicos clienteServicos)
        {
            InitializeComponent();
            cbbTipoConexao.ItemsSource = new List<TipoConexao>()
            {
                TipoConexao.FIREBIRD,
                TipoConexao.SQLSERVER,
                TipoConexao.POSTGRESQL
            };
            pClienteServicos = clienteServicos;
            PreencherCampos(pClienteServicos);
        }

        private void PreencherCampos(ClienteServicos pClienteServicos)
        {
            txtNomeUf.Text = pClienteServicos.Uf;
            txtNomeCliente.Text = pClienteServicos.NomeCliente;
            txtNomeSistema.Text = pClienteServicos.NomeSistema;
            txtNomeServico.Text = pClienteServicos.NomeServico;
            txtNomeDoCaminho.Text = pClienteServicos.ArquivoConfiguracao;
            txtNomeDoArquivo.Text = pClienteServicos.ArquivoExecutavel;
            txtServidor.Text = pClienteServicos.Servidor;
            txtPorta.Text = pClienteServicos.Porta.ToString();
            txtDataBase.Text = pClienteServicos.DataBase;
            cbbTipoConexao.SelectedValue = (TipoConexao)Enum.Parse(typeof(TipoConexao), pClienteServicos.TipoConexao.ToString());
            txtUsuario.Text = pClienteServicos.Usuario;
            txtSenha.Password = pClienteServicos.Senha;
        }

        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
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

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            FrameActive.Content = ActivatorUtilities.CreateInstance<ListaVinculosServico>(App.Services);
        }

        private void BtnGravar_Click(object sender, RoutedEventArgs e)
        {
            PersistirDados();
            Funcoes.AtivarDesativarControles(Botoes, false);
            Funcoes.AtivarDesativarControles(CamposDeServico, false);
            FrameActive.Content = ActivatorUtilities.CreateInstance<ListaVinculosServico>(App.Services);
        }

        private void PersistirDados()
        {
            ClienteServicos clienteServicos = contexto.ClienteServicos.Find(pClienteServicos.IdClienteServico);
            {
                clienteServicos.ArquivoConfiguracao = txtNomeDoCaminho.Text;
                clienteServicos.ArquivoExecutavel = txtNomeDoArquivo.Text;
                clienteServicos.Servidor = txtServidor.Text;
                clienteServicos.Porta = txtPorta.Text.asIFElse();
                clienteServicos.DataBase = txtDataBase.Text;
                clienteServicos.TipoConexao = cbbTipoConexao.SelectedValue.asInt();
                clienteServicos.Usuario = txtUsuario.Text;
                clienteServicos.Senha = txtSenha.Password;
                clienteServicos.Uf = txtNomeUf.Text;  
            }

            contexto.Entry(clienteServicos);
            contexto.SaveChanges();
        }

        private void BtnCaminhoSistema_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            txtNomeDoArquivo.Text = dialog.FileName;
        }
    }
}
