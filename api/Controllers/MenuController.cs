using api_lrpd.Models;
using api_lrpd.Models.DTO.Menu;
using api_lrpd.Services;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api_lrpd.Controllers
{
    public class MenuController : AuthorizedController
    {
        private readonly MenuRepository menuRepository;
        private readonly BuscarMenuAcessoService buscarMenuAcessoService;

        public MenuController(ILogger<MenuController> logger
            , ContextoExecucao contextoExecucao
            , UserManager<IdentityUser> userManager
            , MenuRepository menuRepository
            , BuscarMenuAcessoService buscarMenuAcessoService
            )
            : base(logger, contextoExecucao)
        {
            this.menuRepository = menuRepository;
            this.buscarMenuAcessoService = buscarMenuAcessoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var menusControleAcesso = await buscarMenuAcessoService.GetListaMenu();
            return Ok(menusControleAcesso);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var menus = await menuRepository.GetAllAsync();
            var menuDTO = menus.Adapt<List<MenuGridResponse>>();
            return Ok(menuDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Menu menu)
        {
            await menuRepository.InsertAsync(menu);

            return Ok(menu);
        }

        [HttpPut]
        public async Task<IActionResult> Atualizar([FromBody] Menu menu)
        {
            await menuRepository.UpdateAsync(menu);

            return Ok(menu);
        }

        [HttpDelete]
        public async Task<IActionResult> ApagarAsync(Guid id)
        {
            await menuRepository.DeleteAsync(id);

            return Ok(true);
        }
    }
}

