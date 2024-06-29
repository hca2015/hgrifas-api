using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api_lrpd.Util.Attribute;
using Microsoft.AspNetCore.Identity;

namespace api_lrpd.Models
{
    public class Usuario : Entidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string cpf { get; set; }
        public DateTime dataNascimento { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        [JsonIgnore]
        public string IdentityUserId { get; set; }
        [JsonIgnore]
        public virtual IdentityUser IdentityUser { get; set; }

        public string email { get; set; }
        public string celular { get; set; }

        public virtual Endereco endereco { get; set; }

        public bool aceiteTermos { get; set; }
        public DateTime aceitoEm { get; set; }

        public string avatar { get; set; }
        public string status { get; set; }

        public override DateTime? criado { get; set; }
        public override string criadoPor { get; set; }
        public override DateTime? atualizado { get; set; }
        public override string atualizadoPor { get; set; }
    }
}

