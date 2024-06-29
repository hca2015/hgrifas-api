using System;
using api_lrpd.Models;
using api_lrpd.Models.DTO.Auth;
using api_lrpd.Models.Entity;

namespace api_lrpd.Services
{
    public class ReenviarToken2FAService : BaseServiceAsync
    {
        private ValidarTokenRequest validarTokenRequest;
        private Token2FA token2FA;
        private Token2FAResponse token2FAResponse;
        private readonly Token2FARepository token2FARepository;
        private readonly RegistrarToken2FAService registrarToken2FAService;

        public ReenviarToken2FAService(ILogger<BaseServiceAsync> logger
            , ContextoExecucao contextoExecucao
            , Token2FARepository token2FARepository
            , RegistrarToken2FAService registrarToken2FAService)
            : base(logger, contextoExecucao)
        {
            this.token2FARepository = token2FARepository;
            this.registrarToken2FAService = registrarToken2FAService;
        }

        protected override async Task<bool> PreCondicaoAsync()
        {
            token2FA = await token2FARepository.GetIdAsync(validarTokenRequest.Id);

            if(token2FA is null)
                messageList.AddErro("Token não encontrado para reenvio, refaça o login.");

            return await base.PreCondicaoAsync();
        }

        protected override async Task<bool> ProcessarAsync()
        {
            token2FAResponse = await registrarToken2FAService.GerarToken(token2FA.IdentityUser);

            return await base.ProcessarAsync();
        }

        public async Task<Token2FAResponse> Reenviar(ValidarTokenRequest validarTokenRequest)
        {
            this.validarTokenRequest = validarTokenRequest;

            await ExecutarNoTransactionAsync();

            return token2FAResponse;
        }
    }
}

