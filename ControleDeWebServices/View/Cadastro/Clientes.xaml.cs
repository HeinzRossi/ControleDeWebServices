using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using ControleDeWebServices.View.Cadastro;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Cadastro
{
    public partial class Clientes : Page
    {
        private DadosDBContext contexto;
        public Acao Acao;
        public Cliente idCliente;
        private Frame FrameActive;

        public Clientes()
        {
            Acao = Acao.Inserir;
            InitializeComponent();
            contexto = new DadosDBContext();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
        }
        public Clientes(Cliente IdCliente)
        {
            Acao = Acao.Editar;
            idCliente = IdCliente;
            InitializeComponent();
            contexto = new DadosDBContext();
            BuscarRegistros();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
        }
        private void BtnGravar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!ValidarCampos())
                return;
            PersistirDados(Acao);
            FrameActive.Content = new ListaDeClientes();
        }
        private bool ValidarCampos()
        {
            if ((txtNomeDoCliente.Text.ToString() == null) || (txtNomeDoCliente.Text.ToString() == ""))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Favor informar o nome do cliente!", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    txtNomeDoCliente.Focus();
                    return false;
                }
            }
            if ((cbbUF.Text.ToString() == null) || (cbbUF.Text.ToString() == ""))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Favor informar a UF do cliente!", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    cbbUF.Focus();
                    return false;
                }
            }
            return true;
        }
        private void BtnCancelar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FrameActive.Content = new ListaDeClientes();
        }
        private void PersistirDados(Acao pAcao)
        {
            switch (pAcao)
            {
                case Acao.Inserir:
                    {
                        Cliente clientes = new Cliente()
                        {
                            CodigoControle = txtCodigoControle.Text == null ? 0 : txtCodigoControle.Text.asInt(),
                            NomeCliente = txtNomeDoCliente.Text,
                            Uf = cbbUF.SelectedValue == null ? "" : cbbUF.SelectedValue.ToString()
                        };

                        contexto.Cliente.Add(clientes);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Editar:
                    {

                        var idCliente = ReflectionDataGrid.CastObject<Cliente>(this.idCliente);

                        Cliente clientes = contexto.Cliente.Find(idCliente.IdCliente);
                        {
                            clientes.CodigoControle = txtCodigoControle.Text == null ? 0 : txtCodigoControle.Text.asInt();
                            clientes.NomeCliente = txtNomeDoCliente.Text;
                            clientes.Uf = cbbUF.SelectedValue == null ? "" : cbbUF.SelectedValue.ToString();
                        };

                        contexto.Entry(clientes);
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Excluir:
                    {
                        var idClientes = ReflectionDataGrid.CastObject<Clientes>(idCliente);

                        Sistemas Sistemas = contexto.Sistemas.Find(idClientes.idCliente);
                        contexto.Sistemas.Remove(Sistemas);
                        contexto.SaveChanges();
                        break;
                    }
            }
        }
        private void BuscarRegistros()
        {
            var idCliente = ReflectionDataGrid.CastObject<Cliente>(this.idCliente);

            Cliente clientes = contexto.Cliente.Find(idCliente.IdCliente);
            {
                txtCodigoControle.Text = clientes.CodigoControle.ToString();
                txtNomeDoCliente.Text = clientes.NomeCliente;
                cbbUF.SelectedValue = (Estados)Enum.Parse(typeof(Estados), idCliente.Uf.ToString());
            };
        }
        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
        }
    }
}
