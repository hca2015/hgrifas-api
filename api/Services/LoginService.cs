using api_lrpd.Models;
using api_lrpd.Models.DTO;
using api_lrpd.Models.DTO.Auth;
using api_lrpd.Models.Entity;
using Microsoft.AspNetCore.Identity;

namespace api_lrpd.Services
{
    public class LoginService : BaseServiceAsync
    {
        private LoginRequest loginRequest;
        private IdentityUser user;
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private RegistrarToken2FAService registrarToken2FAService;
        private Token2FAResponse token2FA;

        public LoginService(ILogger<LoginService> logger
            , ContextoExecucao contextoExecucao
            , SignInManager<IdentityUser> signInManager
            , UserManager<IdentityUser> userManager
            , RegistrarToken2FAService registrarToken2FAService 
            )
            : base(logger, contextoExecucao)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.registrarToken2FAService = registrarToken2FAService;
        }

        protected override async Task<bool> PreCondicaoAsync()
        {
            user = await userManager.FindByEmailAsync(loginRequest.Email);

            if(user is null)
            {
                messageList.AddErro("Usuário e/ou senha inválidos");
                return await messageList.WithoutErrorAsync();
            }

            var signInResult = await signInManager.PasswordSignInAsync(user, loginRequest.Senha, true, true);

            if (!signInResult.Succeeded)
            {
                messageList.AddErro("Usuário e/ou senha inválidos");
            }

            return await base.PreCondicaoAsync();
        }

        protected override async Task<bool> ProcessarAsync()
        {
            token2FA = await registrarToken2FAService.GerarToken(user);

            return await base.ProcessarAsync();
        }

        public async Task<Token2FAResponse> Logar(LoginRequest model)
        {
            loginRequest = model;

            await ExecutarNoTransactionAsync();

            return token2FA;
        }
    }
}

