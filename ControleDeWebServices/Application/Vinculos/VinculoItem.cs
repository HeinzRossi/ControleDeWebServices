namespace ControleDeWebServices.Application.Vinculos
{
    public sealed class VinculoItem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Uf { get; set; }
        public bool IsPending { get; set; }
    }
}
