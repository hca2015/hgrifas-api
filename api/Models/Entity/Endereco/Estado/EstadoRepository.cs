using System;
namespace api_lrpd.Models
{
    public class EstadoRepository : BaseRepository<Estado>
    {
        public EstadoRepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
            : base(applicationDbContext, contextoExecucao)
        {
        }
    }
}

