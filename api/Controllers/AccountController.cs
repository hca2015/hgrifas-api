using api_lrpd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_lrpd.Controllers
{
    public class AccountController : AuthorizedController
    {
        private readonly UsuarioRepository usuarioRepository;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            ILogger<AccountController> logger,
            ContextoExecucao contextoExecucao,
            UsuarioRepository usuarioRepository,
            RoleManager<IdentityRole> roleManager)
            : base(logger, contextoExecucao)
        {
            this.usuarioRepository = usuarioRepository;
            this.roleManager = roleManager;
        }        

        [HttpGet("sidenav/user")]
        public async Task<IActionResult> GetUsuarioSidenav()
        {
            Usuario usuario = await usuarioRepository.GetEmailAsync(contextoExecucao.GetLoginUsuario());

            return Ok(usuario);
        }

        [HttpPost("usuario/salvar")]
        public async Task<IActionResult> UpdateUsuario([FromBody] Usuario usuario)
        {
            await usuarioRepository.UpdateAsync(usuario);

            return Ok(usuario);
        }

        [HttpGet("roles")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await roleManager.Roles.ToListAsync());
        }
    }
}

