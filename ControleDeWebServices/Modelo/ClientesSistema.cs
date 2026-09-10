using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeWebServices.Modelo
{
    [Table("ClienteSistemas")]
    public partial class ClienteSistemas: ClasseBaseVinculo
    {
        private bool? _usaCriptografia = false;
        [Key]
        [Required(ErrorMessage = "O campo ID não pode ser nulo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClientesSistema { get; set; }
        public int IdCliente { get; set; }
        public int IdSistemas { get; set; }
        public int IdSecao { get; set; }
        public int QuantidadeAcessos { get; set; }
        public bool? UsaCriptografia { get { return _usaCriptografia; } set { _usaCriptografia = (value == true); } }
    }
}
