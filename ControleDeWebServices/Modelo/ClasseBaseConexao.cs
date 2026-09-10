using System.ComponentModel.DataAnnotations;

namespace ControleDeWebServices.Modelo
{
    public class ClasseBaseConexao
    {
        [MaxLength(50)]
        public string Nome { get; set; }
        [MaxLength(2)]
        public string Uf { get; set; }
    }
}
