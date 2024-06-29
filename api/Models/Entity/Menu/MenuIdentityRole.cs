using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace api_lrpd.Models
{
    public class MenuIdentityRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [ForeignKey(nameof(menu))]
        public Guid menuId { get; set; }
        public Menu menu { get; set; }

        [ForeignKey(nameof(role))]
        public string roleId { get; set; }
        [JsonIgnore]
        public IdentityRole role { get; set; }
        public string roleName { get => role?.NormalizedName; }
    }
}

