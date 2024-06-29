using System;
namespace api_lrpd.Models
{
    public interface IEntidade
    {
        DateTime? criado { get; set; }
        string criadoPor { get; set; }
        DateTime? atualizado { get; set; }
        string atualizadoPor { get; set; }

        void setCriado();
        void setAtualizado();
        void setCriadoPor(string username);
        void setAtualizadoPor(string username);
    }
}

