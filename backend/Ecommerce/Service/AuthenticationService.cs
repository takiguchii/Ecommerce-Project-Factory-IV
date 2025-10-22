using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        // Ferramentas necessárias (injetadas)
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        
        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // --- Implementação do Login ---
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            // Validação de negócio (no Service)
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                // Lança uma exceção que o Controller vai tratar
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            // Gera o token
            var tokenResponse = GenerateJwtToken(user);
            return tokenResponse;
        }

        // --- Implementação do Registro ---
        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            // Validação de negócio (no Service)
            var userExists = await _userManager.FindByNameAsync(dto.Username);
            if (userExists != null)
            {
                // Retorna um erro específico do Identity
                return IdentityResult.Failed(new IdentityError 
                    { Code = "UsernameInUse", Description = "Este nome de usuário já está em uso." });
            }

            var emailExists = await _userManager.FindByEmailAsync(dto.Email);
            if (emailExists != null)
            {
                return IdentityResult.Failed(new IdentityError 
                    { Code = "EmailInUse", Description = "Este e-mail já está cadastrado." });
            }

            // Cria o usuário
            IdentityUser user = new()
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Username
            };

            // Tenta criar e retorna o resultado (sucesso ou falha)
            var result = await _userManager.CreateAsync(user, dto.Password);
            return result;
        }

        // --- Lógica privada de geração de token (só o Service precisa saber) ---
        private LoginResponseDto GenerateJwtToken(IdentityUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var issuer = _configuration["JWT:ValidIssuer"];
            var audience = _configuration["JWT:ValidAudience"];
            var expiration = DateTime.Now.AddHours(3);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: expiration,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}