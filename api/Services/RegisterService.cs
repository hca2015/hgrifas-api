using System;
using System.Transactions;
using api_lrpd.Models;
using api_lrpd.Util.Attribute;
using api_lrpd.Util.Framework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace api_lrpd.Services
{
    public class RegisterService : BaseServiceAsync
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly UsuarioRepository usuarioRepository;
        private readonly LogradouroRepository logradouroRepository;

        public RegisterService(ILogger<RegisterService> logger, ContextoExecucao contextoExecucao, UserManager<IdentityUser> userManager, UsuarioRepository usuarioRepository, LogradouroRepository logradouroRepository)
            : base(logger, contextoExecucao)
        {
            this.userManager = userManager;
            this.usuarioRepository = usuarioRepository;
            this.logradouroRepository = logradouroRepository;
        }

        public async Task Registrar(RegisterRequest model)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {

                var userExists = await userManager.FindByNameAsync(model.Email);
                if (userExists != null)
                    throw new Exception("Usuário já existe");

                IdentityUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Email,
                    PhoneNumber = model.Usuario.celular
                };

                var result = await userManager.CreateAsync(user, model.Senha);
                if (!result.Succeeded)
                    throw new Exception("Erro ao criar usuário => " + string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));

                Usuario u = new()
                {
                    IdentityUser = user,
                    cpf = model.Usuario.cpf,
                    dataNascimento = model.Usuario.dataNascimento.Date,
                    nome = model.Usuario.nome,
                    sobrenome = model.Usuario.sobrenome,
                    endereco = model.Usuario.endereco,
                    celular = model.Usuario.celular,
                    email = model.Email
                };

                usuarioRepository.InsertAsync(u).GetAwaiter().GetResult();

                scope.Complete();
            }
        }
    }
}

