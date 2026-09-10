using ControleDeWebServices.Diversos;
using System;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using ControleDeWebServices.Modelo;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ControleDeWebServices.View.Vinculos
{
    public partial class VinculoClienteServico : Page
    {
        DadosDBContext contexto = new DadosDBContext();
        public Acao Acao;
        private Frame FrameActive;
        private ExibirVinculos exibirVinculos = new ExibirVinculos();
        private ObservableCollection<Servicos> ColecaoServicosDisponiveis;
        private ObservableCollection<Servicos> ColecaoServicosVinculados;
        private List<Servicos> listaDeServicos = new List<Servicos>();
        private ClienteSistemas pClienteSistema = new ClienteSistemas();
        public VinculoClienteServico()
        {
            InitializeComponent();
            cbbUF.ItemsSource = Funcoes.GetEnumValues();
            ExibirClienteVinculos(exibirVinculos);
        }
        public VinculoClienteServico(ClienteSistemas IdCliente)
        {
            Acao = Acao.Editar;
            InitializeComponent();
            pClienteSistema = IdCliente; 
            cbbUF.ItemsSource = Funcoes.GetEnumValues();

            cbbUF.SelectedValue = (Estados)Enum.Parse(typeof(Estados), IdCliente.Uf);
            cbbClientes.SelectedValue = pClienteSistema.NomeCliente;
            cbbSistemas.SelectedValue = pClienteSistema.NomeSistema;

            cbbUF.IsEnabled = false;
            cbbClientes.IsEnabled = false;
            cbbSistemas.IsEnabled = false;
        }
        public void PassarContent(Frame pFrame)
        {
            FrameActive = pFrame;
        }

        private void BtnGravar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PersistirDados(Acao);
            FrameActive.Content = new ListaVinculosServico();
        }

        private void PersistirDados(Acao pAcao)
        {
            Cliente cliente = ReflectionDataGrid.CastObject<Cliente>(cbbClientes.SelectedItem);
            Sistemas sistemas = ReflectionDataGrid.CastObject<Sistemas>(cbbSistemas.SelectedItem);
            switch (Acao)
            {
                case Acao.Inserir:
                {
                    foreach (Servicos item in ListaServicosVinculados.Items)
                    {
                        ClienteServicos clienteServicos = new ClienteServicos()
                        {
                            IdSistemas = sistemas.IdSistemas,
                            IdCliente = cliente.IdCliente,
                            IdServicos = item.IdServicos
                        };
                        contexto.ClienteServicos.Add(clienteServicos);
                    }
                    contexto.SaveChanges();
                    break;
                }
                case Acao.Editar:
                {
                    foreach (Servicos item in listaDeServicos)
                    {
                        switch (item.Status)
                        {
                            case StatusServico.Inserir:
                                {
                                    ClienteServicos clienteServicos = new ClienteServicos()
                                    {
                                        IdSistemas = sistemas.IdSistemas,
                                        IdCliente = cliente.IdCliente,
                                        IdServicos = item.IdServicos
                                    };
                                    contexto.ClienteServicos.Add(clienteServicos);
                                    break;
                                }
                            case StatusServico.Excluir:
                                {
                                        ClienteServicos clienteServicos = contexto.ClienteServicos.Where(
                                                                                                        s => s.IdSistemas == sistemas.IdSistemas &&
                                                                                                        s.IdCliente == pClienteSistema.IdCliente &&
                                                                                                        s.IdServicos == item.IdServicos
                                                                                                        ).FirstOrDefault();
                                    contexto.ClienteServicos.Remove(clienteServicos);
                                    break;
                                };
                        };
                    };

                    contexto.SaveChanges();
                    break;
                }
            }
        }

        private void BtnCancelar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FrameActive.Content = new ListaVinculosServico();
        }

        private void CbbUF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbbUF.SelectedItem == null)
                return;
            BuscarRegistrosCliente(cbbUF.SelectedItem.ToString());
            if (cbbClientes.Items.Count > 0)
            {
                exibirVinculos.Clientes = Visibility.Visible;
            }
            else
            {
                exibirVinculos.Clientes = Visibility.Hidden;
            }
            ExibirClienteVinculos(exibirVinculos);
        }

        private void ExibirClienteVinculos(ExibirVinculos exibirVinculos)
        {
            txtSelecionarCliente.Visibility = exibirVinculos.Clientes;
            cbbClientes.Visibility = exibirVinculos.Clientes;
            txtSelecionarSistemas.Visibility = exibirVinculos.Sistemas;
            cbbSistemas.Visibility = exibirVinculos.Sistemas;
            GrupoSistemasDisponiveis.Visibility = exibirVinculos.ServicosDisponiveis;
            GrupoSistemasVinculados.Visibility = exibirVinculos.ServicosVinculados;
        }

        private void BuscarRegistrosCliente(string pEstado)
        {
            switch (Acao)
            {
                case Acao.Inserir:
                    {
                        var lista = (from e in contexto.Cliente
                                    join clientesistemas in contexto.ClienteSistemas on (e.IdCliente) equals (clientesistemas.IdCliente) into JG
                                    from joineleft in JG.DefaultIfEmpty()
                                    where e.Uf == pEstado && !(from clienteservicos in contexto.ClienteServicos
                                                               where clienteservicos.IdSistemas == joineleft.IdSistemas
                                                             select clienteservicos.IdCliente).Contains(e.IdCliente)
                                    select e).Distinct();
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

        private void CbbClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            exibirVinculos.ServicosDisponiveis = Visibility.Hidden;
            exibirVinculos.ServicosVinculados = Visibility.Hidden;
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
                        BuscarSistemas(pClienteSistema.Uf, pClienteSistema.IdCliente);
                        cbbClientes.SelectedValue = pClienteSistema.NomeCliente;
                        break;
                    }
            }
            ExibirClienteVinculos(exibirVinculos);
        }

        private void BuscarSistemas(string pEstado, int pIdCliente)
        {
            CarregarSistemas(pEstado, pIdCliente);

            if (cbbSistemas.Items.Count > 0)
                exibirVinculos.Sistemas = Visibility.Visible;
            else
                exibirVinculos.Sistemas = Visibility.Hidden;

            ExibirClienteVinculos(exibirVinculos);
        }

        private void CarregarSistemas(string pEstado, int pIdCliente)
        {
            switch (Acao)
            {
                case Acao.Inserir:
                    {
                        var lista = from sistema in contexto.Sistemas
                                    join Clientesistema in contexto.ClienteSistemas on (sistema.IdSistemas) equals (Clientesistema.IdSistemas)
                                    join cliente in contexto.Cliente on (Clientesistema.IdCliente) equals (cliente.IdCliente)
                                    where cliente.IdCliente == pIdCliente && !(from clienteServicos in contexto.ClienteServicos
                                                                               where Clientesistema.IdCliente == clienteServicos.IdCliente
                                                                               select clienteServicos.IdSistemas).Contains(sistema.IdSistemas)
                                    select sistema;

                        cbbSistemas.ItemsSource = lista.ToList();
                        break;
                    }
                case Acao.Editar:
                    {
                        var lista = from sistema in contexto.Sistemas
                                    join Clientesistema in contexto.ClienteSistemas on (sistema.IdSistemas) equals (Clientesistema.IdSistemas)
                                    join cliente in contexto.Cliente on (Clientesistema.IdCliente) equals (cliente.IdCliente)
                                    where cliente.IdCliente == pIdCliente
                                    select sistema;

                        cbbSistemas.ItemsSource = lista.ToList();
                        break;
                    }
            }
        }

        private void CbbSistemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Acao)
            {
                case Acao.Inserir:
                    {
                        if (cbbSistemas.SelectedItem == null)
                            return;

                        Cliente cliente = ReflectionDataGrid.CastObject<Cliente>(cbbClientes.SelectedItem);
                        Sistemas sistemas = ReflectionDataGrid.CastObject<Sistemas>(cbbSistemas.SelectedItem);
                        BuscarServicos(cliente.Uf, cliente.IdCliente, sistemas.IdSistemas);
                        break;
                    }
                case Acao.Editar:
                    {
                        BuscarServicos(pClienteSistema.Uf, pClienteSistema.IdCliente, pClienteSistema.IdSistemas);
                        cbbSistemas.SelectedValue = pClienteSistema.NomeSistema;
                        break;
                    }
            }
            exibirVinculos.ServicosDisponiveis = Visibility.Visible;
            exibirVinculos.ServicosVinculados = Visibility.Visible;

            ExibirClienteVinculos(exibirVinculos);
        }
        private void BuscarServicos(string pUF, int pIdCliente, int pIdSistemas)
        {
            ColecaoServicosDisponiveis = new ObservableCollection<Servicos>(
                                                                                         from servicos in contexto.Servicos
                                                                                         where servicos.Uf == pUF
                                                                                         select servicos);
            ColecaoServicosVinculados = new ObservableCollection<Servicos>(
                                                                             from servicos in contexto.Servicos
                                                                             join Clienteservico in contexto.ClienteServicos on (servicos.IdServicos) equals (Clienteservico.IdServicos)
                                                                             join sistemas in contexto.Sistemas on (Clienteservico.IdSistemas) equals (sistemas.IdSistemas)
                                                                             join cliente in contexto.Cliente on (Clienteservico.IdCliente) equals (cliente.IdCliente)
                                                                             where cliente.IdCliente == pIdCliente && sistemas.IdSistemas == pIdSistemas
                                                                             select servicos);

            foreach (var item in ColecaoServicosVinculados)
            {
                ColecaoServicosDisponiveis.Remove(item);
            }

            foreach (var item in ColecaoServicosVinculados)
            {
                IncluirListaDeServicos(item);
            }
            ListaDeServicos();
        }
        private void IncluirListaDeServicos(Servicos pServico)
        {
            if (!listaDeServicos.Contains(pServico))
                listaDeServicos.Add(pServico);
        }
        private void ListaDeServicos()
        {
            ListaServicosDisponiveis.ItemsSource = ColecaoServicosDisponiveis.ToList();
            ListaServicosVinculados.ItemsSource = ColecaoServicosVinculados.ToList();
        }
        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Servicos pSistema)
            {
                pSistema.Status = StatusServico.Inserir;

                IncluirListaDeServicos(pSistema);
                ColecaoServicosVinculados.Add(pSistema);
                ColecaoServicosDisponiveis.Remove(pSistema);
                ListaDeServicos();
            }
        }

        private void BtnRemover_Click(object sender, RoutedEventArgs e)
        {
            Button cmd = (Button)sender;
            if (cmd.DataContext is Servicos pSistemas)
            {
                pSistemas.Status = StatusServico.Excluir;

                IncluirListaDeServicos(pSistemas);
                ColecaoServicosDisponiveis.Add(pSistemas);
                ColecaoServicosVinculados.Remove(pSistemas);
                ListaDeServicos();
            }
        }
    }
}
