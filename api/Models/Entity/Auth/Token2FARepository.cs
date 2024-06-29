using System;
using Microsoft.EntityFrameworkCore;

namespace api_lrpd.Models.Entity
{
    public class Token2FARepository : BaseRepository<Token2FA>
    {
        public Token2FARepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
            : base(applicationDbContext, contextoExecucao)
        {
        }

        public async Task<Token2FA> GetUserId(string userId)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.IdentityUserId == userId);
        }
    }
}

