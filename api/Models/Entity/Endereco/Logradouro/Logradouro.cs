using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace api_lrpd.Models
{
    public class Logradouro : Entidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        public string cep { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }

        [ForeignKey(nameof(cidade))]
        public long cidadeId { get; set; }
        public virtual Cidade cidade { get; set; }

        public override DateTime? criado { get; set; }
        public override string criadoPor { get; set; }
        public override DateTime? atualizado { get; set; }
        public override string atualizadoPor { get; set; }
    }
}