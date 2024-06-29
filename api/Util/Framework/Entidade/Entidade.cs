using System;
namespace api_lrpd.Models
{
    public abstract class Entidade : IEntidade
    {
        public Entidade()
        {
        }

        public abstract DateTime? criado { get; set; }
        public abstract string criadoPor { get; set; }
        public abstract DateTime? atualizado { get; set; }
        public abstract string atualizadoPor { get; set; }

        public void setAtualizado()
        {
            atualizado = DateTime.Now;
        }

        public void setAtualizadoPor(string username)
        {
            atualizadoPor = username;
        }

        public void setCriado()
        {
            criado = DateTime.Now;
        }

        public void setCriadoPor(string username)
        {
            criadoPor = username;
        }
    }
}

