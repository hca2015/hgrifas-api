using System;
using System.Diagnostics;
using System.Transactions;
using api_lrpd.Models;
using api_lrpd.Util.Attribute;
using api_lrpd.Util.Framework;

namespace api_lrpd.Services
{
    [Transient]
    public class BaseServiceAsync : Classe
    {
        public BaseServiceAsync(ILogger<BaseServiceAsync> logger, ContextoExecucao contextoExecucao)
            : base(logger, contextoExecucao)
        {
        }

        [DebuggerStepThrough]
        protected virtual async Task<bool> PreCondicaoAsync()
        {
            return await messageList.WithoutErrorAsync();
        }

        [DebuggerStepThrough]
        protected virtual async Task<bool> ProcessarAsync()
        {
            return await messageList.WithoutErrorAsync();
        }

        [DebuggerStepThrough]
        protected async Task<bool> ActionAsync()
        {
            if (await PreCondicaoAsync())
            {
                await ProcessarAsync();
            }

            return await messageList.WithoutErrorAsync();
        }

        [DebuggerStepThrough]
        protected async Task<bool> ExecutarAsync()
        {
            TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(1), TransactionScopeAsyncFlowOption.Enabled);
            using (scope)
            {
                try
                {
                    await ActionAsync();
                }
                catch (Exception e)
                {
                    logger.LogCritical(e, "Erro inesperado");
                    messageList.AddErro("Erro inesperado");
                }
            }

            return await messageList.WithoutErrorAsync();
        }

        [DebuggerStepThrough]
        protected async Task<bool> ExecutarNoTransactionAsync()
        {
            try
            {
                await ActionAsync();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Erro inesperado");
                messageList.AddErro("Erro inesperado");
            }

            return await messageList.WithoutErrorAsync();
        }
    }
}

