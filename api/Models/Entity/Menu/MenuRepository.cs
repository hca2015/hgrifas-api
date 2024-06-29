using System;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace api_lrpd.Models
{
    public class MenuRepository : BaseRepository<Menu>
    {
        public MenuRepository(ApplicationDbContext applicationDbContext, ContextoExecucao contextoExecucao)
            : base(applicationDbContext, contextoExecucao)
        {
        }

        private static void TratarCampos(IEntidade entity)
        {
            var menu = entity as Menu;
            menu.type = menu.type?.ToLower().Trim();

            if (!string.IsNullOrWhiteSpace(menu.link))
            {
                menu.link = menu.link.ToLower().Trim();
                if (!menu.link.StartsWith('/'))
                    menu.link = $"/{menu.link}";
            }

            if (menu.children != null && menu.children.Any())
                menu.children.ForEach(TratarCampos);
        }

        protected override async Task<bool> BeforeInsertAsync(IEntidade entity)
        {
            TratarCampos(entity);

            return await base.BeforeInsertAsync(entity);
        }        

        protected override async Task<bool> BeforeUpdateAsync(IEntidade entity)
        {
            TratarCampos(entity);

            return await base.BeforeUpdateAsync(entity);
        }

        public async Task<Menu> GetRota(string s)
        {
            return await dbSet.Include(x => x.children).FirstOrDefaultAsync(x => x.link.StartsWith(s));
        }

        public async Task<Menu> GetTitle(string s)
        {
            return await dbSet.Include(x => x.children).FirstOrDefaultAsync(x => x.title.ToUpper().StartsWith(s.ToUpper()));
        }

        public async Task<List<Menu>> GetAllComIncludeAsync()
        {
            return await dbSet.Where(x => x.menuId == null).Include(x => x.children).ToListAsync();
        }       
    }
}

