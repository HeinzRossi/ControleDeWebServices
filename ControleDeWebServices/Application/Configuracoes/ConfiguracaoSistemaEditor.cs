using System.Collections.Generic;

namespace ControleDeWebServices.Application.Configuracoes
{
    public sealed class ConfiguracaoSistemaEditor
    {
        public int IdClientesSistema { get; set; }
        public int IdCliente { get; set; }
        public int IdSistemas { get; set; }
        public string Uf { get; set; }
        public string NomeCliente { get; set; }
        public string NomeSistema { get; set; }
        public string ArquivoExecutavel { get; set; }
        public string ArquivoConfiguracao { get; set; }
        public string Servidor { get; set; }
        public int Porta { get; set; }
        public string DataBase { get; set; }
        public int TipoConexao { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public int IdSecao { get; set; }
        public bool UsaCriptografia { get; set; }
        public IReadOnlyList<ParametroEditor> Parametros { get; set; }
    }
}
