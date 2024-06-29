using System;
using api_lrpd.Models;
using api_lrpd.Services;

namespace api_lrpd.Util.Framework
{
	public abstract class Classe
	{
        protected readonly ILogger<Classe> logger;
        protected readonly ContextoExecucao contextoExecucao;
        protected MessageList messageList => contextoExecucao.messageList;

        public Classe(ILogger<Classe> logger, ContextoExecucao contextoExecucao)
        {
            this.logger = logger;
            this.contextoExecucao = contextoExecucao;
        }
    }
}

