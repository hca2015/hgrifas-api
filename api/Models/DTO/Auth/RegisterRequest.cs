using System;
using System.ComponentModel.DataAnnotations;

namespace api_lrpd.Models
{
    public class RegisterRequest
    {        
        [EmailAddress]
        [Required(ErrorMessage = "Email requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha requerida")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmar senha requerido")]
        [Compare(nameof(Senha), ErrorMessage = "As senhas não são as mesmas")]
        public string ConfirmarSenha { get; set; }

        [Required(ErrorMessage = "Usuário inválido")]
        public Usuario Usuario { get; set; }
    }
}

