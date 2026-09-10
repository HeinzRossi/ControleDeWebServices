using ControleDeWebServices.Diversos;
using ControleDeWebServices.Modelo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using ComboBox = System.Windows.Controls.ComboBox;

namespace ControleDeWebServices.View.Cadastro
{
    /// <summary>
    /// Lógica interna para Importar.xaml
    /// </summary>
    public partial class ImportarParametrosDe : System.Windows.Window
    {
        private DadosDBContext contexto;
        private ClienteSistemas _clienteSistemas;
        private ObservableCollection<Cliente> ColecaoEstados;
        public ImportarParametrosDe(ClienteSistemas clienteSistemas)
        {
            InitializeComponent();
            contexto = new DadosDBContext();
            _clienteSistemas = clienteSistemas;
            ListaDeEstados(cbbUF);
        }

        private void ListaDeEstados(ComboBox cbbUF)
        {
            List<string> UFEstado = new List<string>();

            ColecaoEstados = new ObservableCollection<Cliente>(
                                                                from estados in contexto.Cliente
                                                                join clientesistemas in contexto.ClienteSistemas on (estados.IdCliente) equals (clientesistemas.IdCliente) into JG
                                                                from joineleft in JG.DefaultIfEmpty()
                                                                where (from clienteservicos in contexto.ClienteServicos
                                                                       where clienteservicos.IdSistemas == joineleft.IdSistemas
                                                                       select clienteservicos.IdCliente).Contains(estados.IdCliente)
                                                                select estados
                                                                );
            foreach (var lista in ColecaoEstados)
            {
                if (!UFEstado.Contains(lista.Uf))
                    UFEstado.Add(lista.Uf);
            };

            UFEstado.Sort();

            cbbUF.ItemsSource = UFEstado;
        }

        private void btnCancelar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        private void cbbUF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuscarRegistrosCliente(cbbUF.SelectedItem.ToString());
        }

        private void BuscarRegistrosCliente(string pEstado)
        {
            var lista = (from e in contexto.Cliente 
                         join clientesistemas in contexto.ClienteSistemas on (e.IdCliente) equals (clientesistemas.IdCliente) into JG
                         from joineleft in JG.DefaultIfEmpty()
                         where e.Uf == pEstado && (from clienteservicos in contexto.ClienteServicos
                                                   where clienteservicos.IdSistemas == joineleft.IdSistemas
                                                   select clienteservicos.IdCliente).Contains(e.IdCliente)
                         select e).Distinct();
            cbbClientes.ItemsSource = lista.ToList();
        }

        private void btnImportar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
            var import = cbbSistemas.SelectedItem.ToType<Import>();

            var listaParametros = (from parametros in contexto.ParametrosSistema
                                   where parametros.IdClientesSistema == import.IdClientesSistema
                                   select parametros);

            foreach (var item in listaParametros) 
            {
                ParametrosSistema parametros = new ParametrosSistema();
                parametros.IdClientesSistema = _clienteSistemas.IdClientesSistema;
                parametros.Secao = item.Secao;
                parametros.Parametro = item.Parametro;
                parametros.Valor = item.Valor;
                
                contexto.ParametrosSistema.Add(parametros);
            }
            contexto.SaveChanges();
            MessageBox.Show("URL atualizada com Sucesso!", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbbClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clienteSelecionado = (Cliente)cbbClientes.SelectedItem;
            var lista = (from clientesistemas in contexto.ClienteSistemas
                         join sistema in contexto.Sistemas on (clientesistemas.IdSistemas) equals (sistema.IdSistemas) into Sis
                         join cliente in contexto.Cliente on (clientesistemas.IdCliente) equals (cliente.IdCliente) into JG
                         join parametros in contexto.ParametrosSistema on (clientesistemas.IdClientesSistema) equals (parametros.IdClientesSistema)
                         from joineleft in Sis.DefaultIfEmpty()
                         where clientesistemas.IdCliente == clienteSelecionado.IdCliente
                         select new { clientesistemas.IdClientesSistema, joineleft.NomeSistema}).Distinct();
            cbbSistemas.ItemsSource = lista.ToList();
        }

        private class Import
        {
            public int IdClientesSistema { get; set; }
            public string NomeSistema { get; set; }
        }
    }
}
