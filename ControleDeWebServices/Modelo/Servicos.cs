namespace ControleDeWebServices.Modelo
{
    using ControleDeWebServices.Diversos;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Servicos")]
    public partial class Servicos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="IdServicos")]
        public int IdServicos { get; set; }
        [MaxLength(50)]
        public string NomeServico { get; set; }
        [MaxLength(2)]
        public string Uf { get; set; }
        [NotMapped]
        public StatusServico Status { get; set; }
        [ForeignKey("IdServicos")]
        public List<ClienteServicos> IdServicosSistema { get; set; }
    }
}
