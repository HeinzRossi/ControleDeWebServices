using ControleDeWebServices.Auxiliar.Custas;
using ControleDeWebServices.Interface;
using ControleDeWebServices.Modelo;
using Engegraph.Comum.Utilitarios.Seguranca;
using PeanutButter.INI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace ControleDeWebServices.Diversos
{
    public class ExecutarSistemaServico : IExecutarSistemaServico
    {
        private INIFile pArquivoConfiguracaoSistema;
        private IQueryable<dynamic> pClienteServicos;
        private XmlSerializer xml;
        private configuration lerConfig;
        private DadosDBContext contexto = new DadosDBContext();
        
        private ClienteSistemas pClienteSistemas { get; set; }

        public ExecutarSistemaServico(ClienteSistemas clienteSistemas)
        {
            pClienteSistemas = clienteSistemas;
        }
        public IExecutarSistemaServico ValidarConfiguracoesSistema()
        {
            if (pClienteSistemas.ArquivoConfiguracao == null)
            {
                throw new ArgumentException("Arquivo de configuração não encontrado!");
            }
            return this;
        }

        public IExecutarSistemaServico ValidarConfiguracoesServico()
        {
            StringBuilder mensagem = new StringBuilder();

            pClienteServicos = (from clienteservicos in contexto.ClienteServicos
                                join servico in contexto.Servicos on clienteservicos.IdServicos equals servico.IdServicos
                                where clienteservicos.IdCliente == pClienteSistemas.IdCliente && clienteservicos.IdSistemas == pClienteSistemas.IdSistemas
                                select new {
                                    clienteservicos.ArquivoConfiguracao,
                                    clienteservicos.ArquivoExecutavel,
                                    clienteservicos.DataBase,
                                    clienteservicos.IdClienteServico,
                                    clienteservicos.IdCliente,
                                    clienteservicos.IdServicos,
                                    clienteservicos.IdSistemas,
                                    servico.NomeServico,
                                    clienteservicos.Porta,
                                    clienteservicos.Senha,
                                    clienteservicos.Servidor,
                                    clienteservicos.TipoConexao,
                                    clienteservicos.Usuario,
                                });

            //foreach (var clienteServicos in pClienteServicos)
            //{
            //    if ((clienteServicos.ArquivoConfiguracao == null) || (clienteServicos.ArquivoConfiguracao == ""))
            //    {
            //        mensagem.Append(String.Concat("Arquivo de configuração do servico de ", clienteServicos.NomeServico, ", não encontrado!", Environment.NewLine));
            //    }

            //}
            //if (mensagem.ToString() != "")
            //{
            //    throw new ArgumentException(mensagem.ToString());
            //}

            return this;
        }

        public IExecutarSistemaServico AtualizarArquivoConfiguracaoSistema()
        {
            pArquivoConfiguracaoSistema = new INIFile(pClienteSistemas.ArquivoConfiguracao);
            TipoConexao tipoConexao     = (TipoConexao)Enum.Parse(typeof(TipoConexao), pClienteSistemas.TipoConexao.ToString());
            var nomeSecao               = RetornaNomeSecao(pClienteSistemas.IdSecao);
            var estado                  = ((Estados)Enum.Parse(typeof(Estados), pClienteSistemas.Uf));
            var usuario                 = string.Empty;
            var senha                   = string.Empty;

            switch (pClienteSistemas.UsaCriptografia)
            {
                case false:
                    {
                        usuario = CriptografiaEng.Criptografar(pClienteSistemas.Usuario);
                        senha = CriptografiaEng.Criptografar(pClienteSistemas.Senha);
                        break;
                    }
                case true:
                    {
                        usuario = pClienteSistemas.Usuario;
                        senha = pClienteSistemas.Senha;
                        break;
                    }
            }

            RenomearSecoes(pArquivoConfiguracaoSistema, nomeSecao);
            ConfigurarSecaoGeral(pArquivoConfiguracaoSistema, tipoConexao);

            switch (tipoConexao)
            {
                case TipoConexao.SQLSERVER:
                    {
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Hostname", CriptografiaEng.Criptografar(pClienteSistemas.Servidor));
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Database", CriptografiaEng.Criptografar(pClienteSistemas.DataBase));
                        switch (estado)
                        {
                            case Estados.MG:
                                {

                                    break;
                                };
                            default:
                                {

                                    pArquivoConfiguracaoSistema.SetValue(nomeSecao, "User_Name", usuario);
                                    pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Password", senha);
                                    break;
                                }
                        }
                        break;
                    }
                case TipoConexao.MSSQL:
                    {
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Hostname", CriptografiaEng.Criptografar(pClienteSistemas.Servidor));
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Database", CriptografiaEng.Criptografar(pClienteSistemas.DataBase));
                        switch (estado)
                        {
                            case Estados.MG:
                                {

                                    break;
                                }
                                ;
                            default:
                                {

                                    pArquivoConfiguracaoSistema.SetValue(nomeSecao, "User_Name", usuario);
                                    pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Password", senha);
                                    break;
                                }
                        }
                        break;
                    }
                case TipoConexao.FIREBIRD:
                    {
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "database", CriptografiaEng.Criptografar(MontarStringConexaoSistema(pClienteSistemas)));
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "User_Name", usuario);
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Password", senha);
                        break;
                    }
                case TipoConexao.ORACLE:
                    {
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "HostName", CriptografiaEng.Criptografar(pClienteSistemas.Servidor));
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "DataBaseCEP", CriptografiaEng.Criptografar(pClienteSistemas.DataBase));
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "User_Name", usuario);
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Password", senha);
                        break;
                    }
                case TipoConexao.POSTGRESQL:
                    {
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "HostName", CriptografiaEng.Criptografar(pClienteSistemas.Servidor));
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "database", CriptografiaEng.Criptografar(pClienteSistemas.DataBase));
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "User_Name", usuario);
                        pArquivoConfiguracaoSistema.SetValue(nomeSecao, "Password", senha);
                        break;
                    }
            }
            pArquivoConfiguracaoSistema.Persist();
            return this;
        }

        private void ConfigurarSecaoGeral(INIFile pArquivoConfiguracaoSistema, TipoConexao pTipoConexao)
        {
            if (!pArquivoConfiguracaoSistema.Sections.Contains("Geral"))
            {
                return;
            }
            switch (pTipoConexao)
            {
                case TipoConexao.FIREBIRD:
                    {
                        pArquivoConfiguracaoSistema.SetValue("Geral", "ConnectionName", "IBLocal");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "DriverName", "DevartInterBase");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "LibraryName", "dbexpida.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "VendorLib", "fbclient.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "GetDriverFunc", "getSQLDriverInterBase");
                        
                        break;
                    }
                case TipoConexao.SQLSERVER:
                    {
                        pArquivoConfiguracaoSistema.SetValue("Geral", "ConnectionName", "SQLSERVER");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "DriverName", "DevartSQLServer");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "LibraryName", "dbexpsda.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "VendorLib", "sqloledb.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "GetDriverFunc", "getSQLDriverSQLServer");
                        break;
                    }
                case TipoConexao.POSTGRESQL:
                    {
                        pArquivoConfiguracaoSistema.SetValue("Geral", "ConnectionName", "POSTGRESQL");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "DriverName", "POSTGRESQL");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "LibraryName", "dbexppgsql.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "VendorLib", "dbexppgsql.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "GetDriverFunc", "getSQLDriverPostgreSQL");
                        break;
                    }
                case TipoConexao.MSSQL: 
                    {
                        pArquivoConfiguracaoSistema.SetValue("Geral", "ConnectionName", "MSSQL");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "DriverName", "DevartSQLServer");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "LibraryName", "dbexpsda.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "VendorLib", "sqloledb.dll");
                        pArquivoConfiguracaoSistema.SetValue("Geral", "GetDriverFunc", "getSQLDriverSQLServer");
                        break;
                    }
            }
            pArquivoConfiguracaoSistema.Persist();
        }

        private void RenomearSecoes(INIFile pArquivoConfiguracaoSistema, string nomeSecao)
        {
            var secoes = (from _secao in contexto.Secao
                         where _secao.NomeSecao != nomeSecao
                         select _secao);

            foreach (var pSecao in secoes)
            {
                foreach (var pArquivoConfiguracao in pArquivoConfiguracaoSistema.Sections)
                {
                   if (pArquivoConfiguracao.ToUpper() == pSecao.NomeSecao.ToUpper())
                    {
                        pArquivoConfiguracaoSistema.RenameSection(pSecao.NomeSecao, nomeSecao);
                        pArquivoConfiguracaoSistema.Persist();
                        break;
                    }
                }
            }
        }

        private string RetornaNomeSecao(int idSecao)
        {
            Secao secao = contexto.Secao.Find(idSecao);

            if (secao == null)
            {
                throw new ArgumentException(string.Format("Nome da seção não definido {0}para o arquivo de configuração do Sistema",Environment.NewLine));
            }
            return secao.NomeSecao;
        }

        private string MontarStringConexaoSistema(ClienteSistemas pClienteSistemas)
        {
            if (pClienteSistemas.Porta == 0) 
                return String.Concat(pClienteSistemas.Servidor,":",pClienteSistemas.DataBase);
            else
                return String.Concat(pClienteSistemas.Servidor,"/",pClienteSistemas.Porta.ToString(),":",pClienteSistemas.DataBase);
        }

        public IExecutarSistemaServico AtualizarArquivoConfiguracaoServico()
        {
            foreach (var clienteservicos in pClienteServicos)
            {
                ClienteServicos servicos = ReflectionDataGrid.CastObject<ClienteServicos>(clienteservicos);
                switch (BuscarTipoServico(clienteservicos.ArquivoConfiguracao))
                {
                    case TipoServico.Custas:
                        {
                            AlimentarXmlCustas(lerConfig,servicos);
                            break;
                        }
                    case TipoServico.Selos:
                        {
                            AlimentarXmlSelos(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.NFSe:
                        {
                            AlimentarXmlNFSe(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.Launcher:
                        {
                            AlimentarXmlLauncher(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.SelosMG:
                        {
                            AlimentarXmlSelosMG(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.Caixa:
                        {
                            AlimentarXmlCaixa(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.ServicosRTD:
                        {
                            AlimentarXmlServicosRTD(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.SelosPA:
                        {
                            AlimentarXmlSelosPA(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.SelosTO:
                        {
                            AlimentarXmlSelosTO(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.SelosMA:
                        {
                            AlimentarXmlSelosMA(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.ServicoRC:
                        {
                            AlimentarXmlServicosRC(lerConfig, servicos);
                            break;
                        }
                    case TipoServico.SAECNacional:
                        {
                            AlimentarXmlServicosSaec(lerConfig, servicos);
                            break;
                        }
                }
            }
            return this;
        }

        private TipoServico BuscarTipoServico(string pArquivoConfiguracao)
        {
            if (string.IsNullOrEmpty(pArquivoConfiguracao))
                return TipoServico.Selos;

            xml = new XmlSerializer(typeof(configuration));
            StreamReader reader = new StreamReader(pArquivoConfiguracao);
            lerConfig = (configuration)xml.Deserialize(reader);
            reader.Close();

            if (lerConfig.CustasProtesto != null)
                return TipoServico.Custas;
            else if (lerConfig.appSettings != null)
                return TipoServico.Selos;
            else if (lerConfig.NFSe != null)
                return TipoServico.NFSe;
            else if (lerConfig.Launcher != null)
                return TipoServico.Launcher;
            else if (lerConfig.SeloDigitalMg != null)
                return TipoServico.SelosMG;
            else if (lerConfig.CaixaServidor != null)
                return TipoServico.Caixa;
            else if (lerConfig.ServicoRTD != null)
                return TipoServico.ServicosRTD;
            else if (lerConfig.SeloDigitalPA != null)
                return TipoServico.SelosPA;
            else if (lerConfig.SeloDigitalTO != null)
                return TipoServico.SelosTO;
            else if (lerConfig.SeloDigitalMA != null)
                return TipoServico.SelosMA;
            else if (lerConfig.ServicoRC != null)
                return TipoServico.ServicoRC;
            else if (lerConfig.SAECNacionalServidor != null)
                return TipoServico.SAECNacional;
            else
                return TipoServico.Selos;
        }
        private void AlimentarXmlServicosSaec(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.SAECNacionalServidor.ServidorBancoDeDadosSAECSqlServer.value = MontarStringConexaoServico(pClienteServicos);
            pConfiguration.SAECNacionalServidor.UsuarioBancoDeDadosSqlServer.value = pClienteServicos.Usuario;
            pConfiguration.SAECNacionalServidor.SenhaBancoDeDadosSqlServer.value = pClienteServicos.Senha;
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlServicosRC(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.ServicoRC.ConexaoBanco.value = MontarStringConexaoServico(pClienteServicos);
            pConfiguration.ServicoRC.UsuarioBanco.value = pClienteServicos.Usuario;
            pConfiguration.ServicoRC.SenhaBanco.value = pClienteServicos.Senha;
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlSelosMA(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
        }
        private void AlimentarXmlSelosTO(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.SeloDigitalTO.SqlHost.value = pClienteServicos.Servidor;
            pConfiguration.SeloDigitalTO.SqlDatabase.value = pClienteServicos.DataBase;
            pConfiguration.SeloDigitalTO.SqlUser.value = pClienteServicos.Usuario;
            pConfiguration.SeloDigitalTO.SqlSenha.value = pClienteServicos.Senha;
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlSelosPA(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.SeloDigitalPA.ConexaoBanco.value = MontarStringConexaoServico(pClienteServicos); ;
            pConfiguration.SeloDigitalPA.UsuarioBanco.value = pClienteServicos.Usuario;
            pConfiguration.SeloDigitalPA.SenhaBanco.value = pClienteServicos.Senha;
            pConfiguration.SeloDigitalPA.TipoDeConexao.value = pClienteServicos.TipoConexao.ToString();
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlServicosRTD(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.ServicoRTD.ConexaoBanco.value = MontarStringConexaoServico(pClienteServicos);
            pConfiguration.ServicoRTD.UsuarioBanco.value = pClienteServicos.Usuario;
            pConfiguration.ServicoRTD.SenhaBanco.value = pClienteServicos.Senha;
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlCaixa(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.CaixaServidor.SqlHost.value = pClienteServicos.Servidor;
            pConfiguration.CaixaServidor.SqlDatabase.value = pClienteServicos.DataBase;
            pConfiguration.CaixaServidor.SqlUser.value = pClienteServicos.Usuario;
            pConfiguration.CaixaServidor.SqlSenha.value = pClienteServicos.Senha;
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlCustas(configuration pConfiguration,ClienteServicos pClienteServicos)
        {
            pConfiguration.CustasProtesto.ConexaoBanco.value = MontarStringConexaoServico(pClienteServicos);
            pConfiguration.CustasProtesto.UsuarioBanco.value = pClienteServicos.Usuario;
            pConfiguration.CustasProtesto.SenhaBanco.value = pClienteServicos.Senha;
            pConfiguration.CustasProtesto.TipoDeConexao.value = pClienteServicos.TipoConexao.ToString();
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlSelos(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            if (string.IsNullOrEmpty(pClienteServicos.ArquivoConfiguracao))
                return;

            pConfiguration.appSettings.FirstOrDefault(x => x.key == "ConexaoBanco").value = CriptografiaEng.Criptografar(MontarStringConexaoServico(pClienteServicos));
            pConfiguration.appSettings.FirstOrDefault(x => x.key == "UsuarioBanco").value = CriptografiaEng.Criptografar(pClienteServicos.Usuario);
            pConfiguration.appSettings.FirstOrDefault(x => x.key == "SenhaBanco").value = CriptografiaEng.Criptografar(pClienteServicos.Senha);
            pConfiguration.appSettings.FirstOrDefault(x => x.key == "TipoDeConexao").value = pClienteServicos.TipoConexao.ToString();
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlNFSe(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.NFSe.SqlHost.value = pClienteServicos.Servidor;
            pConfiguration.NFSe.SqlDatabase.value = pClienteServicos.DataBase;
            pConfiguration.NFSe.Usuario.value = pClienteServicos.Usuario;
            pConfiguration.NFSe.Senha.value = pClienteServicos.Senha;
            pConfiguration.Salvar(pClienteServicos.ArquivoConfiguracao);
        }
        private void AlimentarXmlLauncher(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.Launcher.SqlHost.value = pClienteServicos.Servidor;
            pConfiguration.Launcher.SqlDatabase.value = pClienteServicos.DataBase;
            pConfiguration.Launcher.Usuario.value = pClienteServicos.Usuario;
            pConfiguration.Launcher.Senha.value = pClienteServicos.Senha;
        }
        private void AlimentarXmlSelosMG(configuration pConfiguration, ClienteServicos pClienteServicos)
        {
            pConfiguration.SeloDigitalMg.ConexaoBanco.value = pClienteServicos.Servidor;
            pConfiguration.SeloDigitalMg.Usuario.value = pClienteServicos.Usuario;
            pConfiguration.SeloDigitalMg.Senha.value = pClienteServicos.Senha;
            pConfiguration.SeloDigitalMg.TipoDeConexao.value = pClienteServicos.TipoConexao.ToString();
        }
        private string MontarStringConexaoServico(ClienteServicos pClienteServicos)
        {
            TipoConexao tipoConexao = (TipoConexao)Enum.Parse(typeof(TipoConexao), pClienteServicos.TipoConexao.ToString());

            switch (tipoConexao)
            {
                case TipoConexao.SQLSERVER:
                    {
                        return String.Concat("SERVER=", pClienteServicos.Servidor, ";Database=", pClienteServicos.DataBase);
                    }
                case TipoConexao.FIREBIRD:
                    {
                        if (pClienteServicos.Porta == 0)
                            return String.Concat(pClienteServicos.Servidor, ":", pClienteServicos.DataBase);
                        else
                            return String.Concat(pClienteServicos.Servidor, "/", pClienteServicos.Porta.ToString(), ":", pClienteServicos.DataBase);
                    }
                default:
                    return String.Empty;
            }
        }
        public IExecutarSistemaServico ExecutarServicos()
        {
            foreach (var clienteservicos in pClienteServicos)
            {
                ClienteServicos servicos = ReflectionDataGrid.CastObject<ClienteServicos>(clienteservicos);
                if (servicos.ArquivoExecutavel != "")
                    Process.Start(servicos.ArquivoExecutavel);
            }
            return this;
        }

        public IExecutarSistemaServico DerrubarServicos()
        {
            Process[] processos = Process.GetProcesses();
            foreach (Process processo in processos)
            {
                if (processo.ProcessName.Length >= 11)
                    if (processo.ProcessName.Substring(0, 11) == "SeloDigital")
                        processo.Kill();
                if (processo.ProcessName.Length >= 15)
                    if (processo.ProcessName.Substring(0, 15) == "CustasProtestos")
                        processo.Kill();
                if (processo.ProcessName.Length >= 4)
                    if (processo.ProcessName.Substring(0, 4) == "NFSe")
                        processo.Kill();
                if (processo.ProcessName.Length >= 11)
                    if (processo.ProcessName.Substring(0, 11) == "ServicosRTD")
                        processo.Kill();
                if (processo.ProcessName.Length >= 5)
                    if (processo.ProcessName.Substring(0, 5) == "Caixa")
                        processo.Kill();
                if (processo.ProcessName.Length >= 19)
                    if (processo.ProcessName.Substring(0, 19) == "CadastroDeEnderecos")
                        processo.Kill();
                if (processo.ProcessName.Length >= 8)
                    if (processo.ProcessName.Substring(0, 8) == "Launcher")
                        processo.Kill();
                if (processo.ProcessName.Length >= 11)
                    if (processo.ProcessName.Substring(0,9) == "ServicoRC")
                        processo.Kill();
                if (processo.ProcessName.Length >= 12)
                    if (processo.ProcessName.Substring(0, 12) == "SAECNacional")
                        processo.Kill();
            }
            return this;
        }

        public IExecutarSistemaServico AtualizarQuantidadeDeAcessos()
        {
            ClienteSistemas clienteSistemas = contexto.ClienteSistemas.Find(pClienteSistemas.IdClientesSistema);
            {
                clienteSistemas.QuantidadeAcessos = clienteSistemas.QuantidadeAcessos + 1;
            }

            contexto.Entry(clienteSistemas);
            contexto.SaveChanges();
            
            return this;
        }

        public IExecutarSistemaServico AtualizarParametros()
        {
            try
            {
                AtualizarPadroes atualizarPadroes = new AtualizarPadroes();

                atualizarPadroes.AtualizarParametros(pClienteSistemas, contexto);

            }
            catch (Exception E)
            {
                throw new ArgumentException("Erro na atualização dos Dados!" + "\n" + E.Message);

            }
            return this;
        }
    }
}
