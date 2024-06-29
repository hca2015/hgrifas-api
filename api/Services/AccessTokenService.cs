
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_lrpd.Models;
using api_lrpd.Models.DTO;
using api_lrpd.Models.Entity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace api_lrpd.Services
{
    public class AccessTokenService : BaseServiceAsync
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<IdentityUser> userManager;
        private readonly UsuarioRepository usuarioRepository;
        private IdentityUser user;
        private AccessTokenResponse tokenResponse;

        public AccessTokenService(ILogger<AccessTokenService> logger
            , ContextoExecucao contextoExecucao
            , IConfiguration configuration
            , UserManager<IdentityUser> userManager
            , UsuarioRepository usuarioRepository) : base(logger, contextoExecucao)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.usuarioRepository = usuarioRepository;
        }

        protected override async Task<bool> ProcessarAsync()
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var userRoles = await userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512)
            );

            Usuario usuario = await usuarioRepository.GetIdentityId(user.Id);

            tokenResponse = new AccessTokenResponse()
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                usuario = usuario.Adapt<UsuarioResponse>()
            };

            return await base.ProcessarAsync();
        }

        public async Task<AccessTokenResponse> CriarToken(IdentityUser user)
        {
            this.user = user;

            await ExecutarNoTransactionAsync();

            return tokenResponse;
        }
    }
}

