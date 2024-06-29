using System;
using System.Linq;
using api_lrpd.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace api_lrpd.Services
{
    public class BuscarMenuAcessoService : BaseServiceAsync
    {
        private readonly MenuRepository menuRepository;
        private readonly UserManager<IdentityUser> userManager;
        private List<MenuResponse> menusRetorno;

        public BuscarMenuAcessoService(ILogger<BaseServiceAsync> logger
            , ContextoExecucao contextoExecucao
            , MenuRepository menuRepository
            , UserManager<IdentityUser> userManager)
            : base(logger, contextoExecucao)
        {
            this.menuRepository = menuRepository;
            this.userManager = userManager;
        }

        protected override async Task<bool> ProcessarAsync()
        {
            List<MenuResponse> todosMenus = (await menuRepository.GetAllComIncludeAsync()).Adapt<List<MenuResponse>>();
            List<MenuResponse> menusControleAcesso = new();

            var user = await userManager.FindByEmailAsync(contextoExecucao.GetLoginUsuario());
            var userRoles = await userManager.GetRolesAsync(user);

            if (userRoles.Count > 0)
            {
                if (userRoles.Contains("ADMIN"))
                {
                    menusRetorno = todosMenus;
                    return true;
                }

                var linq = from tm in todosMenus
                           where tm.roles.Where(userRoles.Contains).Any()
                           select tm;

                menusRetorno = linq.ToList();
            }

            menusRetorno.AddRange(todosMenus.Where(x => x.roles.Count == 0));

            return await base.ProcessarAsync();
        }

        public async Task<List<MenuResponse>> GetListaMenu()
        {
            await ExecutarNoTransactionAsync();

            return menusRetorno;
        }
    }
}

