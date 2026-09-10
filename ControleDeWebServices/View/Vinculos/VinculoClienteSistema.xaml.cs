using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ControleDeWebServices.View.Vinculos
{

    public partial class VinculoClienteSistema : Page
    {
        DadosDBContext contexto = new DadosDBContext();
        public Acao Acao;
        public Sistemas idSistemas;
        private Cliente pCliente = new Cliente();
        private ObservableCollection<Sistemas> ColecaoSistemasDisponiveis;
        private ObservableCollection<Sistemas> ColecaoSistemasVinculados;
        private List<Sistemas> listaDeSistemas = new List<Sistemas>();
        private Frame FrameActive;
        public VinculoClienteSistema()
        {
            Acao = Acao.Inserir;
            InitializeComponent();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
            ExibirClienteVinculos();
        }
        public VinculoClienteSistema(Cliente IdCliente)
        {
            Acao = Acao.Editar;
            InitializeComponent();
            pCliente = IdCliente;
            cbbUF.ItemsSource = Funcoes.GetEnumValues();

            cbbUF.SelectedValue = (Estados)Enum.Parse(typeof(Estados), IdCliente.Uf);
            cbbClientes.SelectedValue = pCliente.NomeCliente;

            cbbUF.IsEnabled = false;
            cbbClientes.IsEnabled = false;
        }

        private void ExibirClienteVinculos(Visibility pCliente = Visibility.Hidden , Visibility pVinculos = Visibility.Hidden)
        {
            txtSelecionarCliente.Visibility = pCliente;
            cbbClientes.Visibility = pCliente;
            GrupoSistemasDisponiveis.Visibility = pVinculos;
            GrupoSistemasVinculados.Visibility = pVinculos;
        }

        private void BuscarRegistros(string pEstado)
        {
            switch (Acao)
            {
                case Acao.Inserir:
                    {
                        var lista = from e in contexto.Cliente
                                    where e.Uf == pEstado && !(from clientesistema in contexto.ClienteSistemas
                                                               select clientesistema.IdCliente).Contains(e.IdCliente)
                                    select e;
                        cbbClientes.ItemsSource = lista.ToList();
                        break;
                    }
                case Acao.Editar:
                    {
                        var lista = from e in contexto.Cliente
                                    where e.Uf == pEstado 
                                    select e;
                        cbbClientes.ItemsSource = lista.ToList();
                        break;
                    }
            }
        }

        private void BtnGravar_Click(object sender, RoutedEventArgs e)
        {
            PersistirDados(Acao);
            FrameActive.Content = new ListaVinculosSistema();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            FrameActive.Content = new ListaVinculosSistema();
        }
        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Sistemas pSistema)
            {
                pSistema.Status = StatusServico.Inserir;

                IncluirListaDeSistemas(pSistema);
                ColecaoSistemasVinculados.Add(pSistema);
                ColecaoSistemasDisponiveis.Remove(pSistema);
                ListaDeSistemas();
            }
        }

        private void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Sistemas pSistemas)
            {
                pSistemas.Status = StatusServico.Excluir;

                IncluirListaDeSistemas(pSistemas);
                ColecaoSistemasDisponiveis.Add(pSistemas);
                ColecaoSistemasVinculados.Remove(pSistemas);
                ListaDeSistemas();
            }
        }
        private void CbbUF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbUF.SelectedItem == null)
                return;
            BuscarRegistros(cbbUF.SelectedItem.ToString());
            if (cbbClientes.Items.Count > 0)
                ExibirClienteVinculos(Visibility.Visible);
            else
                ExibirClienteVinculos(Visibility.Hidden);
        }

        private void CbbClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Acao)
            {
                case Acao.Inserir:
                    {
                        if (cbbClientes.SelectedItem == null)
                            return;

                        Cliente cliente = ReflectionDataGrid.CastObject<Cliente>(cbbClientes.SelectedItem);
                        BuscarSistemas(cliente.Uf, cliente.IdCliente);
                        break;
                    }
                case Acao.Editar:
                    {
                        BuscarSistemas(pCliente.Uf, pCliente.IdCliente);
                        cbbClientes.SelectedValue = pCliente.NomeCliente;
                        break;
                    }
            }
            ExibirClienteVinculos(Visibility.Visible, Visibility.Visible);
                
        }
        private void BuscarSistemas(string pUF,int pIdCliente)
        {
            ColecaoSistemasDisponiveis = new ObservableCollection<Sistemas>(
                                                                             from sistema in contexto.Sistemas
                                                                             where sistema.Uf == pUF
                                                                             select sistema);

            ColecaoSistemasVinculados = new ObservableCollection<Sistemas>(
                                                                             from sistema in contexto.Sistemas
                                                                             join Clientesistema in contexto.ClienteSistemas on (sistema.IdSistemas) equals (Clientesistema.IdSistemas)
                                                                             join cliente in contexto.Cliente on (Clientesistema.IdCliente) equals (cliente.IdCliente)
                                                                             where cliente.IdCliente == pIdCliente
                                                                             select sistema);
            foreach (var item in ColecaoSistemasVinculados)
            {
                ColecaoSistemasDisponiveis.Remove(item);
            }

            foreach (var item in ColecaoSistemasVinculados)
            {
                IncluirListaDeSistemas(item);
            }
            ListaDeSistemas();
        }
        private void IncluirListaDeSistemas(Sistemas pSistema)
        {
            if (listaDeSistemas.Contains(pSistema))
                listaDeSistemas.Add(pSistema);
        }
        private void ListaDeSistemas()
        {
            ListaSistemasDisponiveis.ItemsSource = ColecaoSistemasDisponiveis.ToList();
            ListaSistemasVinculados.ItemsSource = ColecaoSistemasVinculados.ToList();
        }
        private void PersistirDados(Acao pAcao)
        {
            Cliente cliente = ReflectionDataGrid.CastObject<Cliente>(cbbClientes.SelectedItem);
            switch (pAcao)
            {
                case Acao.Inserir:
                    {
                        foreach (Sistemas item in ListaSistemasVinculados.Items)
                        {
                            ClienteSistemas clienteSistemas = new ClienteSistemas()
                            {
                                IdSistemas = item.IdSistemas,
                                IdCliente = cliente.IdCliente
                            };
                            contexto.ClienteSistemas.Add(clienteSistemas);
                        };
                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Editar:
                    {

                        foreach (Sistemas item in listaDeSistemas)
                        {
                            switch (item.Status)
                            {
                                case StatusServico.Inserir:
                                    {
                                        ClienteSistemas clienteSistemas = new ClienteSistemas()
                                        {
                                            IdSistemas = item.IdSistemas,
                                            IdCliente  = cliente.IdCliente
                                        };
                                        contexto.ClienteSistemas.Add(clienteSistemas);
                                        break;
                                    }
                                case StatusServico.Excluir:
                                    {
                                        ClienteSistemas clienteSistemas = contexto.ClienteSistemas.Where(
                                                                                                            s => s.IdSistemas == item.IdSistemas &&
                                                                                                            s.IdCliente == pCliente.IdCliente
                                                                                                          ).FirstOrDefault();
                                        contexto.ClienteSistemas.Remove(clienteSistemas);
                                        break;
                                    };
                            };
                        };

                        contexto.SaveChanges();
                        break;
                    }
                case Acao.Excluir:
                    {
                        var idServico = ReflectionDataGrid.CastObject<Sistemas>(idSistemas);

                        Sistemas Sistemas = contexto.Sistemas.Find(idServico.IdSistemas);
                        ClienteSistemas clienteSistema = contexto.ClienteSistemas.Find(idServico.IdSistemas);
                        contexto.Sistemas.Remove(Sistemas);
                        contexto.ClienteSistemas.Remove(clienteSistema);
                        contexto.SaveChanges();
                        break;
                    }
            }
        }
    }
}
