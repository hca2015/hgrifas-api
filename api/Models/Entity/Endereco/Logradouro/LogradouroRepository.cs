using System;
namespace api_lrpd.Models
{
    public class LogradouroRepository : BaseRepository<Logradouro>
    {
        public LogradouroRepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
            : base(applicationDbContext, contextoExecucao)
        {
        }

        public Logradouro GetCep(string cep)
        {
            return dbSet.FirstOrDefault(x => x.cep == cep);
        }
    }
}

