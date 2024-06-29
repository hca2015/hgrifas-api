using System;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.Extensions.Logging;

namespace api_lrpd.Util.Framework.Middlewares
{
    public static class ErrorMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, Serilog.ILogger logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        string mensagem = "Erro inesperado";

                        if (contextFeature.Error != null)
                        {
                            logger.Error(contextFeature.Error.MensagemExcecaoStackTrace());
                            mensagem = contextFeature.Error.MensagemExcecao();
                        }

                        await context.Response.WriteAsync(new
                        {
                            error = new List<string>() { mensagem }
                        }.ToString());
                    }
                });
            });
        }
    }
}

