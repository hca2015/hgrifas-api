using System;
using api_lrpd.Util.Attribute;

namespace api_lrpd.Models
{
    [Scoped]
    public class ContextoExecucao
    {
        public readonly IHttpContextAccessor httpContextAcessor;
        public readonly IServiceProvider serviceProvider;
        public readonly IConfiguration configuration;
        public readonly MessageList messageList;
        private bool isAdmin = false;

        public ContextoExecucao(IHttpContextAccessor httpContext, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            httpContextAcessor = httpContext;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            messageList = new MessageList();
            SetarAdmin();
        }

        private void SetarAdmin()
        {
            if(bool.TryParse(httpContextAcessor?.HttpContext?.User?.Identity.IsAuthenticated.ToString(), out bool autenticado) && autenticado)
            {
                isAdmin = httpContextAcessor.HttpContext.User.IsInRole("ADMIN");
            }
        }

        public string GetLoginUsuario()
        {
            return httpContextAcessor?.HttpContext?.User?.Identity?.Name;
        }

        public bool UserIsAdmin()
        {
            return isAdmin;
        }
    }
}

