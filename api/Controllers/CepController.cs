using api_lrpd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_lrpd.Controllers
{
    public class CepController : AuthorizedController
    {
        private readonly LogradouroRepository logradouroRepository;

        public CepController(
            ILogger<CepController> logger,
            ContextoExecucao contextoExecucao
            , LogradouroRepository logradouroRepository)
            : base(logger, contextoExecucao)
        {
            this.logradouroRepository = logradouroRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public Logradouro Get(string cep)
        {
            return this.logradouroRepository.GetCep(cep);
        }
    }
}

