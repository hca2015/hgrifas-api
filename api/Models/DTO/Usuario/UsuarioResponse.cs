using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_lrpd.Models.DTO
{
    public class UsuarioResponse
    {
        public Guid id { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string cpf { get; set; }
        public DateTime dataNascimento { get; set; }

        public string email { get; set; }
        public string celular { get; set; }

        public virtual Endereco endereco { get; set; }

        public string avatar { get; set; }
        public string status { get; set; }
    }
}

