using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace api_lrpd.Models
{
    public class Endereco : Entidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string numero { get; set; }
        public string complemento { get; set; }

        [ForeignKey(nameof(logradouro))]
        public long logradouroId { get; set; }
        public virtual Logradouro logradouro { get; set; }

        [ForeignKey(nameof(usuario))]
        public Guid usuarioId { get; set; }
        public virtual Usuario usuario { get; set; }
                
        public override DateTime? criado { get; set; }
        public override string criadoPor { get; set; }
        public override DateTime? atualizado { get; set; }
        public override string atualizadoPor { get; set; }
    }
}