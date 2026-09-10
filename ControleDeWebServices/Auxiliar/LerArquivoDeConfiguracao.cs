using Engegraph.Comum.Utilitarios.Seguranca;
using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ControleDeWebServices.Auxiliar.Custas
{

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class configuration
    {
        public CustasProtesto CustasProtesto { get; set; }
        [XmlArrayItemAttribute("add", IsNullable = false)]
        public ConfigurationAdd[] appSettings { get; set; }
        public configurationNFSe NFSe { get; set; }
        public configurationLauncher Launcher { get; set; }
        public configurationSeloDigitalMg SeloDigitalMg { get; set; }
        public CaixaServidor CaixaServidor { get; set; }
        public ServicoRTD ServicoRTD { get; set; }
        public configurationSeloDigitalPA SeloDigitalPA { get; set; }
        public ConfigurationSeloDigitalTO SeloDigitalTO { get; set; }
        public ConfigurationSeloDigitalMA SeloDigitalMA { get; set; }
        public ConfigurationServicoRC ServicoRC { get; set; }
        public ConfigurationSAECNacionalServidor SAECNacionalServidor { get; set; }
        public void Salvar(string pNomeArquivo)
        {
            using (var stream = new StreamWriter(pNomeArquivo, append: false, Encoding.UTF8))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer XML = new XmlSerializer(typeof(configuration));
                XML.Serialize(stream, this, ns);
            }
        }
    }
    #region Custas
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class CustasProtesto
    {
        public CustasProtestoConexaoBanco ConexaoBanco { get; set; }
        public CustasProtestoUsuarioBanco UsuarioBanco { get; set; }
        public CustasProtestoSenhaBanco SenhaBanco { get; set; }
        public CustasProtestoAttributo Token { get; set; }
        public CustasProtestoAttributo ChaveEmolumentoArquivamento { get; set; }
        public CustasProtestoAttributo TipoDeConexao { get; set; }
        public CustasProtestoAttributo PastaImagensIndexadas { get; set; }
        public CustasProtestoAttributo ApiUsuarioUnicred { get; set; }
        public CustasProtestoAttributo ApiSenhaUnicred { get; set; }
        public CustasProtestoAttributo ApiKeyUnicred { get; set; }
        public CustasProtestoAttributo ApiCooperativaUnicred { get; set; }
        public CustasProtestoAttributo ApiBeneficiarioIdUnicred { get; set; }
        public CustasProtestoAttributo ApiAmbienteUnicred { get; set; }
        public CustasProtestoAttributo ApiBeneficiarioVariacaoCarteira { get; set; }
        public CustasProtestoAttributo BaseRTD { get; set; }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class CustasProtestoConexaoBanco
    {
        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = CriptografiaEng.Criptografar(value);
            }
        }
    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class CustasProtestoUsuarioBanco
    {
        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = CriptografiaEng.Criptografar(value);
            }
        }
    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class CustasProtestoSenhaBanco
    {
        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = CriptografiaEng.Criptografar(value);
            }
        }
    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class CustasProtestoAttributo
    {

        [XmlAttributeAttribute()]
        public string value { get; set; }
    }
    #endregion
    #region Selos
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationAdd
    {
        [XmlAttributeAttribute()]
        public string key { get; set; }
        [XmlAttributeAttribute()]
        public string value { get; set; }
    }
    #endregion
    #region NFSe
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationNFSe
    {

        public configurationNFSeAttributo CpfCnpj { get; set; }
        public configurationNFSeAttributo InscricaoMunicipal { get; set; }
        public configurationNFSeAttributo CodigoTributacaoMunicipio { get; set; }
        public configurationNFSeAttributo CodigoMunicipio { get; set; }
        public configurationNFSeAttributo NomeCertificado { get; set; }
        public configurationNFSeAttributo SenhaCertificado { get; set; }
        public configurationNFSeHost SqlHost { get; set; }
        public configurationNFSeDataBase SqlDatabase { get; set; }
        public configurationNFSeUsuario Usuario { get; set; }
        public configurationNFSeSenha Senha { get; set; }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationNFSeHost
    {
        private string FValue;
        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationNFSeDataBase
    {
        private string FValue;
        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationNFSeUsuario
    {
        private string FValue;
        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationNFSeSenha
    {
        private string FValue;
        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationNFSeAttributo
    {

        [XmlAttributeAttribute()]
        public string value { get; set; }
    }
    #endregion
    #region Launcher
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationLauncher
    {

        public configurationLauncherSqlHost SqlHost { get; set; }
        public configurationLauncherSqlDatabase SqlDatabase { get; set; }
        public configurationLauncherUsuario Usuario { get; set; }
        public configurationLauncherSenha Senha { get; set; }
        public configurationLauncherAttributo TipoSgbd { get; set; }
        public configurationLauncherAttributo AmbienteAtualizacao { get; set; }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationLauncherSqlHost
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationLauncherSqlDatabase
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }
    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationLauncherUsuario
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationLauncherSenha
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationLauncherAttributo
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = value;
            }
        }
    }
    #endregion
    #region Selos_MG
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalMg
    {

        public configurationSeloDigitalMgConexao ConexaoBanco { get; set; }
        public configurationSeloDigitalMgConexao Usuario { get; set; }
        public configurationSeloDigitalMgConexao Senha { get; set; }
        public configurationSeloDigitalMgAttributo CodigoServentia { get; set; }
        public configurationSeloDigitalMgAttributo Token { get; set; }
        public configurationSeloDigitalMgAttributo TipoDeConexao { get; set; }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalMgConexao
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = CriptografiaEng.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalMgAttributo
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = value;
            }
        }
    }
    #endregion
    #region Caixa
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class CaixaServidor
    {

        public configurationCaixaConexaoBanco SqlHost { get; set; }
        public configurationCaixaConexaoBanco SqlDatabase { get; set; }
        public configurationCaixaConexaoBanco SqlUser { get; set; }
        public configurationCaixaConexaoBanco SqlSenha { get; set; }
        public configurationCaixaConexaoConfiguracao GerarNotaFiscal { get; set; }

    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationCaixaConexaoBanco
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }

    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationCaixaConexaoConfiguracao
    {

        [XmlAttributeAttribute()]
        public string value { get; set; }

    }
    #endregion
    #region ServicosRTD
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ServicoRTD
    {

        public configurationServicoRTDConexaoBanco ConexaoBanco { get; set; }
        public configurationServicoRTDConexaoBanco UsuarioBanco { get; set; }
        public configurationServicoRTDConexaoBanco SenhaBanco { get; set; }
        public configurationServicoRTDConfiguracao Token { get; set; }
        public configurationServicoRTDConfiguracao Uf { get; set; }
        public configurationServicoRTDConfiguracao CodigoTipoDocumento { get; set; }
        public configurationServicoRTDConfiguracao CodigoTipoEmolumentoZonaUrbanaGoiania { get; set; }
        public configurationServicoRTDConfiguracao CodigoTipoEmolumentoZonaRuralGoiania { get; set; }
        public configurationServicoRTDConfiguracao CodigoTipoEmolumentoZonaUrbana { get; set; }
        public configurationServicoRTDConfiguracao PastaArquivosEmPdf { get; set; }
        public configurationServicoRTDConfiguracao TipoDeConexao { get; set; }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationServicoRTDConexaoBanco
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return FValue;
            }
            set
            {
                FValue = XAesSimples.Criptografar(value);
            }
        }
    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationServicoRTDConfiguracao
    {

        [XmlAttributeAttribute()]
        public string value { get; set; }
    }
    #endregion
    #region Selos_PA
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalPA
    {

        public configurationSeloDigitalPAConexao ConexaoBanco { get; set; }
        public configurationSeloDigitalPAConexao UsuarioBanco { get; set; }
        public configurationSeloDigitalPAConexao SenhaBanco { get; set; }
        public configurationSeloDigitalPaAttributo AmbienteDeAtualizacao { get; set; }
        public configurationSeloDigitalPaAttributo TipoDeConexao { get; set; }
        public configurationSeloDigitalPaAttributo AmbienteProducao { get; set; }
        public configurationSeloDigitalPaAttributo AmbienteProducaoTJPA { get; set; }
        public configurationSeloDigitalPaAttributo Token { get; set; }
        public configurationSeloDigitalPaAttributo CaminhoArquivoXml { get; set; }

    }

    /// <remarks/>
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalPAConexao
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = CriptografiaEng.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalPaAttributo
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = value;
            }
        }
    }
    #endregion
    #region Selos_TO
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationSeloDigitalTO
    {
        public configurationSeloDigitalTOAttributo Token { get; set; }
        public configurationSeloDigitalTOConexao SqlHost { get; set; }

        public configurationSeloDigitalTOConexao SqlDatabase { get; set; }

        public configurationSeloDigitalTOConexao SqlUser { get; set; }

        public configurationSeloDigitalTOConexao SqlSenha { get; set; }

        public configurationSeloDigitalTOAttributo Ambiente { get; set; }

        public configurationSeloDigitalTOAttributo DesabilitarExportacaoManual { get; set; }

    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalTOConexao
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = XAesSimples.Criptografar(value);
                }  
        }
    }
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalTOAttributo
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = value;
            }
        }
    }
    
    #endregion
    #region SelosMA
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationSeloDigitalMA
    {
        private configurationSeloDigitalMAConexao urlWebSiteField { get; set; }
        private configurationSeloDigitalMAConexao loginField { get; set; }
        private configurationSeloDigitalMAConexao senhaField { get; set; }
        private configurationSeloDigitalMAConexao cienteIdField { get; set; }
        private configurationSeloDigitalMAConexao tokenField { get; set; }
    }

    /// <remarks/>
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class configurationSeloDigitalMAConexao
    {

        private string FValue;
        [XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return FValue;
            }
            set
            {
                this.FValue = XAesSimples.Criptografar(value); ;
            }
        }
    }
    #endregion
    #region ServicoRC
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationServicoRC
    {

        public ConfigurationServicoRCConexao ConexaoBanco;

        public ConfigurationServicoRCConexao UsuarioBanco;

        public ConfigurationServicoRCConexao SenhaBanco;

        public ConfigurationServicoRCAttributo Token;

        public ConfigurationServicoRCAttributo TipoDeConexao;
    }

    /// <remarks/>
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationServicoRCConexao
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = XAesSimples.Criptografar(value);
            }
        }
    }

    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationServicoRCAttributo
    {

        private string FValue;

        [XmlAttributeAttribute()]
        public string value
        {
            get
            {
                return this.FValue;
            }
            set
            {
                this.FValue = value;
            }
        }
    }
    #endregion

    #region SAEC

    /// <remarks/>
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ConfigurationSAECNacionalServidor
    {

        public ConfigurationSAECNacionalServidorAttributo AmbienteDeAtualizacao { get; set;}

        public ConfigurationSAECNacionalServidorAttributo AmbienteDeProducao { get; set;}

        public ConfigurationSAECNacionalServidorAttributo IdDoClienteControle { get; set;}

        public configurationSAECNacionalServidorConexao ServidorBancoDeDadosSAECSqlServer { get; set;}

        public configurationSAECNacionalServidorConexao UsuarioBancoDeDadosSqlServer { get; set;}

        public configurationSAECNacionalServidorConexao SenhaBancoDeDadosSqlServer { get; set;}

        public configurationSAECNacionalServidorConexao ServidorBancoDeDadosAnexoSqlServer { get; set;}

        public ConfigurationSAECNacionalServidorAttributo EnderecoDeServicoSeloDigital { get; set;}

        public ConfigurationSAECNacionalServidorAttributo TipoBancoDeDadosRI { get; set;}

        public ConfigurationSAECNacionalServidorAttributo ServidorBancoDeDadosRI { get; set;}

        public ConfigurationSAECNacionalServidorAttributo UsuarioBancoDeDadosRI { get; set;}

        public ConfigurationSAECNacionalServidorAttributo SenhaBancoDeDadosRI { get; set;}

        public ConfigurationSAECNacionalServidorAttributo TokenSeloDigital { get; set;}

        public ConfigurationSAECNacionalServidorAttributo UfEmExecucao { get; set;}


        [SerializableAttribute()]
        [DesignerCategoryAttribute("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class ConfigurationSAECNacionalServidorAttributo
        {
            [XmlAttributeAttribute()]
            public string value { get; set; }
        }
        /// <remarks/>
        [SerializableAttribute()]
        [DesignerCategoryAttribute("code")]
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class configurationSAECNacionalServidorConexao
        {

            private string FValue;

            [XmlAttributeAttribute()]
            public string value
            {
                get
                {
                    return this.FValue;
                }
                set
                {
                    this.FValue = XAesSimples.Criptografar(value);
                }
            }
        }
    }
    #endregion
}

