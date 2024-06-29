using api_lrpd.Models;
using api_lrpd.Models.DTO;
using api_lrpd.Models.DTO.Auth;
using api_lrpd.Models.Entity;

namespace api_lrpd.Services
{
    public class ValidarToken2FAService : BaseServiceAsync
    {
        private readonly AccessTokenService accessTokenService;
        private readonly Token2FARepository token2FARepository;
        private ValidarTokenRequest validarTokenRequest;
        private AccessTokenResponse accessToken;
        private Token2FA token2FA;

        public ValidarToken2FAService(ILogger<ValidarToken2FAService> logger
            , ContextoExecucao contextoExecucao
            , AccessTokenService tokenService
            , Token2FARepository token2FARepository)
            : base(logger, contextoExecucao)
        {
            this.accessTokenService = tokenService;
            this.token2FARepository = token2FARepository;
        }

        protected override async Task<bool> PreCondicaoAsync()
        {
            token2FA = await token2FARepository.GetIdAsync(validarTokenRequest.Id);

            if(token2FA.IsExpirado || !token2FA.Token.Equals(validarTokenRequest.Token))
                 messageList.AddErro("Token inválido e/ou expirado.");

            return await base.PreCondicaoAsync();
        }

        protected override async Task<bool> ProcessarAsync()
        {
            accessToken = await accessTokenService.CriarToken(token2FA.IdentityUser);

            await token2FARepository.DeleteAsync(token2FA);

            return await base.ProcessarAsync();
        }

        public async Task<AccessTokenResponse> ValidarToken(ValidarTokenRequest model)
        {
            validarTokenRequest = model;

            await ExecutarNoTransactionAsync();

            return accessToken;
        }
    }
}

