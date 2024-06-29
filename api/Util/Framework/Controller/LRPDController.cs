using api_lrpd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api_lrpd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LRPDController : ControllerBase
{
    protected readonly ILogger<LRPDController> _logger;
    protected readonly ContextoExecucao contextoExecucao;

    public LRPDController(ILogger<LRPDController> logger, ContextoExecucao contextoExecucao)
    {
        _logger = logger;
        this.contextoExecucao = contextoExecucao;
    }    
}

