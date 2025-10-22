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
 
        private readonly UserManager<IdentityUser> _userManager;
        
        private readonly RoleManager<IdentityRole> _roleManager; 
        private readonly IConfiguration _configuration;
        
        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration)
        {
            _userManager = userManager; 
            _roleManager = roleManager; 
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

        // --- MÉTODO REGISTER (MODIFICADO) ---
        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            // Validações (intactas)
            var userExists = await _userManager.FindByNameAsync(dto.Username);
            if (userExists != null)
            {
                return IdentityResult.Failed(new IdentityError 
                    { Code = "UsernameInUse", Description = "Este nome de usuário já está em uso." });
            }
            var emailExists = await _userManager.FindByEmailAsync(dto.Email);
            if (emailExists != null)
            {
                return IdentityResult.Failed(new IdentityError 
                    { Code = "EmailInUse", Description = "Este e-mail já está cadastrado." });
            }

            // Cria o usuário (intacto)
            IdentityUser user = new()
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Username
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            // --- ESTA É A NOVA LÓGICA ---
            // 4. Se a criação do usuário tiver SUCESSO
            if (result.Succeeded)
            {
                // 5. Define o perfil padrão
                string defaultRole = "User";

                // 6. Verifica se o perfil "User" já existe
                var roleExist = await _roleManager.RoleExistsAsync(defaultRole);
                if (!roleExist)
                {
                    // 7. Se não existir, / Agora esta linha funcionaCRIE O PERFIL "User" (e "Admin")
                    await _roleManager.CreateAsync(new IdentityRole(defaultRole));
                    await _roleManager.CreateAsync(new IdentityRole("Admin")); 
                }

                // 8. Adiciona o novo usuário ao perfil "User"
                await _userManager.AddToRoleAsync(user, defaultRole);
            }
            
            return result;
        }

        // --- Lógica privada de geração de token (só o Service precisa saber) ---
        private LoginResponseDto GenerateJwtToken(IdentityUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // (AINDA VAMOS ADICIONAR OS PERFIS AQUI)
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
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256) // Você tinha HmacSha256Ldap, mas o padrão é HmacSha256
            );

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
