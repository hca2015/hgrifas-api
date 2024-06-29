using System;
using System.Diagnostics;

namespace api_lrpd.Models
{
    [DebuggerStepThrough]
    public class MessageList : List<Mensagem>
	{
        public async Task<bool> WithErrorAsync()
        {
            return await Task.FromResult(WithError());
        }

        public bool WithError()
        {
            return this.Any(x => x.type == Mensagem.Tipo.ERROR || x.type == Mensagem.Tipo.FATAL);
        }

        public async Task<bool> WithoutErrorAsync()
        {
            return await Task.FromResult(WithoutError());
        }

        public bool WithoutError()
        {
            return !this.Any(x => x.type == Mensagem.Tipo.ERROR || x.type == Mensagem.Tipo.FATAL);
        }

        public void AddErro(string v)
        {
            Add(new Mensagem() { detail = v, type = Mensagem.Tipo.ERROR });
        }
    }
}

