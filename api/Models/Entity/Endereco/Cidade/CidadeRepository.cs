using System;
namespace api_lrpd.Models
{
    public class CidadeRepository : BaseRepository<Cidade>
    {
        public CidadeRepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
            : base(applicationDbContext, contextoExecucao)
        {
        }
    }
}

