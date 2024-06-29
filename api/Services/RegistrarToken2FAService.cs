using System;
using api_lrpd.Models;
using api_lrpd.Models.Entity;
using api_lrpd.Util;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace api_lrpd.Services
{
    public class RegistrarToken2FAService : BaseServiceAsync
    {
        private readonly Token2FARepository token2FARepository;
        private IdentityUser user;
        private Token2FAResponse token;

        public RegistrarToken2FAService(ILogger<BaseServiceAsync> logger
            , ContextoExecucao contextoExecucao
            , Token2FARepository token2FARepository)
            : base(logger, contextoExecucao)
        {
            this.token2FARepository = token2FARepository;
        }

        protected override async Task<bool> PreCondicaoAsync()
        {
            if (user is null)
                messageList.AddErro("Usuário não encontrado para registar token");

            return await base.PreCondicaoAsync();
        }

        protected override async Task<bool> ProcessarAsync()
        {
            Token2FA tokenExistente = await token2FARepository.GetUserId(user.Id);

            if(tokenExistente != null)
            {
                if(!await token2FARepository.DeleteAsync(tokenExistente))
                    throw new Exception("Erro ao remover token existente");
            }

            Token2FA token2FA = new()
            {
                Expiracao = DateTime.Now.AddMinutes(5),
                Token = Utility.GerarNDigitos(6),
                IdentityUser = user
            };

            await token2FARepository.InsertAsync(token2FA);

            token = token2FA.Adapt<Token2FAResponse>();

            return await base.ProcessarAsync();
        }

        public async Task<Token2FAResponse> GerarToken(IdentityUser user)
        {
            this.user = user;

            await ExecutarNoTransactionAsync();

            return token;
        }
    }
}

