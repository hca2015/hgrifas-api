using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace api_lrpd.Models
{
    public class Menu : Entidade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public string title { get; set; } //'Example',
        public string type { get; set; } //'basic',
        public string icon { get; set; } //'heroicons_outline:chart-pie',
        public string link { get; set; } //'/example'

        public bool visible { get; set; }

        public virtual List<MenuIdentityRole> roles { get; set; }

        public Guid? menuId { get; set; }
        public virtual List<Menu> children { get; set; }

        public override DateTime? criado { get; set; }
        public override string criadoPor { get; set; }
        public override DateTime? atualizado { get; set; }
        public override string atualizadoPor { get; set; }
    }
}

