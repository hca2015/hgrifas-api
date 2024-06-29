using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using api_lrpd.Util.Attribute;
using static System.Net.Mime.MediaTypeNames;

namespace api_lrpd.Util
{
    public static class Utility
    {
        public static WebApplicationBuilder ConfigurarServices(this WebApplicationBuilder builder)
        {
            Type scopedRegistration = typeof(ScopedAttribute);
            Type singletonRegistration = typeof(SingletonAttribute);
            Type transientRegistration = typeof(TransientAttribute);

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsDefined(scopedRegistration, true) || p.IsDefined(transientRegistration, true) || p.IsDefined(singletonRegistration, true) && !p.IsInterface).Select(s => new
                {
                    Interface = s.GetInterface($"I{s.Name}"),
                    Implementation = s
                }).Where(x => x.Implementation != null);

            foreach (var type in types)
            {
                if (type.Implementation.IsDefined(scopedRegistration, false) || type.Implementation.BaseType.IsDefined(scopedRegistration, false))
                {
                    if (type.Interface != null)
                        builder.Services.AddScoped(type.Interface, type.Implementation);
                    else
                        builder.Services.AddScoped(type.Implementation);

                    continue;
                }

                if (type.Implementation.IsDefined(transientRegistration, false) || type.Implementation.BaseType.IsDefined(transientRegistration, false))
                {
                    if (type.Interface != null)
                        builder.Services.AddTransient(type.Interface, type.Implementation);
                    else
                        builder.Services.AddTransient(type.Implementation);

                    continue;
                }

                if (type.Implementation.IsDefined(singletonRegistration, false) || type.Implementation.BaseType.IsDefined(singletonRegistration, false))
                {
                    if (type.Interface != null)
                        builder.Services.AddSingleton(type.Interface, type.Implementation);
                    else
                        builder.Services.AddSingleton(type.Implementation);

                    continue;
                }
            }

            return builder;
        }

        public static string MensagemExcecao(this Exception exception)
        {
            string retorno = string.Empty;

            retorno += exception.Message + Environment.NewLine;

            if (exception.InnerException != null)
                retorno += MensagemExcecao(exception.InnerException);

            return retorno;
        }

        public static string MensagemExcecaoStackTrace(this Exception exception)
        {
            string retorno = string.Empty;

            retorno += exception.Message + Environment.NewLine;
            retorno += exception.StackTrace + Environment.NewLine;

            if (exception.InnerException != null)
                retorno += MensagemExcecaoStackTrace(exception.InnerException);

            return retorno;
        }

        public static string GerarNDigitos(int quantidadeDigitos)
        {
            string retorno = string.Empty;

            for (int i = 0; i < quantidadeDigitos; i++)
            {
                retorno += RandomNumberGenerator.GetInt32(10);
            }
            return retorno;
        }

        public static string RemoverCaracteresEspeciais(this string input)
        {
            // Remove os caracteres especiais
            string normalized = Regex.Replace(input, @"[^0-9a-zA-Z\s]", "");

            // Substitui os acentos e cedilha
            string result = new(normalized
                                .Normalize(NormalizationForm.FormD)
                                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                                .ToArray());

            return result;
        }
    }
}

