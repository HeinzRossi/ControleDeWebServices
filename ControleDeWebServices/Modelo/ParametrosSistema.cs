using ControleDeWebServices.Diversos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeWebServices.Modelo
{
    [Table("ParametrosSistema")]
    public class ParametrosSistema
    {
        [Key]
        [Required(ErrorMessage = "O campo ID não pode ser nulo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdParametrosSistema { get; set; }
        public int IdClientesSistema { get; set; }
        [MaxLength(300)]
        public string Secao { get; set; }
        [MaxLength(300)]
        public string Parametro { get; set; }
        [MaxLength(300)]
        public string Valor { get; set; }
        [NotMapped]
        public StatusServico Status { get; set; }
    }
}
