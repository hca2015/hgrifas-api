using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_lrpd.Models
{
    public class Cidade : Entidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string nome { get; set; }

        [ForeignKey(nameof(estado))]
        public long estadoId { get; set; }
        public virtual Estado estado { get; set; }

        public override DateTime? criado { get; set; }
        public override string criadoPor { get; set; }
        public override DateTime? atualizado { get; set; }
        public override string atualizadoPor { get; set; }
    }
}