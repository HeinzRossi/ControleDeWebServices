using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControleDeWebServices
{
    public class ClasseBaseVinculo
    {
        [NotMapped]
        public string NomeCliente { get; set; }
        [NotMapped]
        public string NomeSistema { get; set; }
        [NotMapped]
        public string NomeServico { get; set; }
        [NotMapped]
        public string Uf { get; set; }
        [MaxLength(200)]
        public string DataBase { get; set; }
        [MaxLength(200)]
        public string ArquivoExecutavel { get; set; }
        [MaxLength(200)]
        public string ArquivoConfiguracao { get; set; }
        [Range(0, int.MaxValue)]
        public int Porta { get; set; }
        [MaxLength(20)]
        public string Senha { get; set; }
        [MaxLength(50)]
        public string Servidor { get; set; }
        [Range(0, int.MaxValue)]
        public int TipoConexao { get; set; }
        [MaxLength(20)]
        public string Usuario { get; set; }
    }
}