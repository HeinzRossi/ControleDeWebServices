namespace ControleDeWebServices.Modelo
{
    using ControleDeWebServices.Diversos;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Sistemas")]
    public partial class Sistemas
    {
        [Key]
        [Required(ErrorMessage = "O campo ID não pode ser nulo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "IdSistemas")]
        public int IdSistemas { get; set; }
        [MaxLength(50)]
        public string NomeSistema { get; set; }
        [MaxLength(2)]
        public string Uf { get; set; }
        [NotMapped]
        public StatusServico Status { get; set; }
        [ForeignKey("IdSistemas")]
        public List<ClienteServicos> IdServicosSistema { get; set; }
        [ForeignKey("IdSistemas")]
        public List<ClienteSistemas> IdClientesSistema { get; set; }
    }
}
