using System;
using System.IdentityModel.Tokens.Jwt;
using api_lrpd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace api_lrpd.Controllers
{
    [Authorize]
    public class AuthorizedController : LRPDController
    {
        public AuthorizedController(ILogger<AuthorizedController> logger, ContextoExecucao contextoExecucao)
            : base (logger, contextoExecucao)
        {            
        }        
    }
}

