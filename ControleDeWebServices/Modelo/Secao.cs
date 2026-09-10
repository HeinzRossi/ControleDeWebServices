using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeWebServices.Modelo
{
    public partial class Secao
    {
        [Key]
        [Required(ErrorMessage = "O campo ID não pode ser nulo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "IdSecao")]
        public int IdSecao { get; set; }
        [MaxLength(50)]
        public string NomeSecao { get; set; }

    }
}
