using api_lrpd.Models;
using api_lrpd.Models.DTO.Auth;
using api_lrpd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api_lrpd.Controllers
{
    [AllowAnonymous]
	public class AuthController : LRPDController
	{
        private readonly RegisterService registerService;
        private readonly ValidarToken2FAService validarToken2FAService;
        private LoginService loginService;
        private readonly ReenviarToken2FAService reenviarToken2FAService;
        private readonly AccessTokenService accessTokenService;
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(ILogger<AuthController> logger
            , ContextoExecucao contextoExecucao
            , RegisterService registerService
            , ValidarToken2FAService validarToken2FAService
            , AccessTokenService accessTokenService
            , UserManager<IdentityUser> userManager
            , LoginService loginService
            , ReenviarToken2FAService reenviarToken2FAService) : base(logger, contextoExecucao)
        {
            this.registerService = registerService;
            this.loginService = loginService;
            this.reenviarToken2FAService = reenviarToken2FAService;
            this.accessTokenService = accessTokenService;
            this.userManager = userManager;
            this.validarToken2FAService = validarToken2FAService;
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest model)
        {
            var token2FA = await loginService.Logar(model);
            return token2FA != null ? Ok(token2FA) : Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            await registerService.Registrar(model);

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token2FA([FromBody] ValidarTokenRequest validarTokenRequest)
        {
            var accessToken = await validarToken2FAService.ValidarToken(validarTokenRequest);
            return accessToken != null ? Ok(accessToken) : Unauthorized();            
        }

        [Authorize]
        [HttpPost("sign-in-with-token")]
        public async Task<IActionResult> SignInToken()
        {
            var identidade = HttpContext.User.Identity;
            if (identidade.IsAuthenticated)
            {
                var usuario = await userManager.FindByEmailAsync(identidade.Name);
                var accessToken = await accessTokenService.CriarToken(usuario);
                return accessToken == null ? Unauthorized() : Ok(accessToken);
            }
            return Unauthorized();
        }

        [HttpPost("reenviar-token")]
        public async Task<IActionResult> ReenviarToken([FromBody] ValidarTokenRequest validarTokenRequest)
        {
            var token2FAResponse = await reenviarToken2FAService.Reenviar(validarTokenRequest);

            return Ok(token2FAResponse);
        }
    }
}

