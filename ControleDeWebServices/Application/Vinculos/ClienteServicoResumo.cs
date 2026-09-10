namespace ControleDeWebServices.Application.Vinculos
{
    public sealed class ClienteServicoResumo
    {
        public int IdClienteServico { get; set; }
        public int IdCliente { get; set; }
        public int IdSistemas { get; set; }
        public int IdServicos { get; set; }
        public string NomeCliente { get; set; }
        public string NomeSistema { get; set; }
        public string NomeServico { get; set; }
        public string Uf { get; set; }
    }
}
