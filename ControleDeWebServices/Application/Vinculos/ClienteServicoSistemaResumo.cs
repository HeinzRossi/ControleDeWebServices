namespace ControleDeWebServices.Application.Vinculos
{
    public sealed class ClienteServicoSistemaResumo
    {
        public int IdCliente { get; set; }
        public int IdSistemas { get; set; }
        public string NomeCliente { get; set; }
        public string NomeSistema { get; set; }
        public string Uf { get; set; }
    }
}
