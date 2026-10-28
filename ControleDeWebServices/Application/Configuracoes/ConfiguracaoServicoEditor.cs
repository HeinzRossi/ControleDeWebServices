namespace ControleDeWebServices.Application.Configuracoes
{
    public sealed class ConfiguracaoServicoEditor
    {
        public int IdClienteServico { get; set; }
        public int IdCliente { get; set; }
        public int IdSistemas { get; set; }
        public int IdServicos { get; set; }
        public string Uf { get; set; }
        public string NomeCliente { get; set; }
        public string NomeSistema { get; set; }
        public string NomeServico { get; set; }
        public string ArquivoExecutavel { get; set; }
        public string ArquivoConfiguracao { get; set; }
        public string Servidor { get; set; }
        public int Porta { get; set; }
        public string DataBase { get; set; }
        public int TipoConexao { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
    }
}
