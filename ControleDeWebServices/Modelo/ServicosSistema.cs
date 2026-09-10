using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ControleDeWebServices.Modelo
{
    [Table("ClienteServicos")]
    public class ClienteServicos: ClasseBaseVinculo
    {
        [Key]
        [Required(ErrorMessage = "O campo ID não pode ser nulo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClienteServico { get; set; }
        public int IdSistemas { get; set; }
        public int IdServicos { get; set; }
        public int IdCliente { get; set; }
    }
}
