using System;
using System.ComponentModel.DataAnnotations;

namespace api_lrpd.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email obrigatorio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória")]
        public string Senha { get; set; }
    }
}

