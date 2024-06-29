using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace api_lrpd.Models.Entity
{
    public class Token2FA : Entidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime Expiracao { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        public string IdentityUserId { get; set; }
        public virtual IdentityUser IdentityUser { get; set; }

        public override DateTime? criado { get; set; }
        public override string criadoPor { get; set; }
        public override DateTime? atualizado { get; set; }
        public override string atualizadoPor { get; set; }
        public bool IsExpirado { get => DateTime.Now > Expiracao; }
    }
}

