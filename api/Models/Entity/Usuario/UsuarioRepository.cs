using System;
using Microsoft.EntityFrameworkCore;

namespace api_lrpd.Models
{
    public class UsuarioRepository : BaseRepository<Usuario>
    {
        public UsuarioRepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
            : base(applicationDbContext, contextoExecucao)
        {
        }

        public async Task<Usuario> GetEmailAsync(string email)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.email == email);
        }

        public async Task<Usuario> GetIdentityId(string userId)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.IdentityUserId == userId);
        }
    }
}

