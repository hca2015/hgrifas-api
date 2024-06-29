using System;
namespace api_lrpd.Models
{
    public class EnderecoRepository : BaseRepository<Endereco>
    {
        public EnderecoRepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
            : base(applicationDbContext, contextoExecucao)
        {
        }
    }
}

