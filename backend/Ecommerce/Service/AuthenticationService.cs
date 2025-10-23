using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;

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

        // Login 
        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
            }

            // Chama o método que gera o token (e inclui os perfis)
            var tokenResponse = await GenerateJwtToken(user);
            return tokenResponse;
        }

        // registroo 
        public async Task<IdentityResult> RegisterAsync(RegisterDto dto)
        {
            var userExists = await _userManager.FindByNameAsync(dto.Username);
            if (userExists != null)
            {
                return IdentityResult.Failed(new IdentityError 
                    { Code = "UsernameInUse", Description = "Nome de usuário já em uso." });
            }
            var emailExists = await _userManager.FindByEmailAsync(dto.Email);
            if (emailExists != null)
            {
                return IdentityResult.Failed(new IdentityError 
                    { Code = "EmailInUse", Description = "E-mail já cadastrado." });
            }

            IdentityUser user = new()
            {
                Email = dto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = dto.Username
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                // Adiciona o novo usuário ao perfil "User"
                // (Não precisa checar se existe, ja criei na program)
                await _userManager.AddToRoleAsync(user, "User");
            }
            
            return result;
        }

        // Gerador de tokens com perfil 
        private async Task<LoginResponseDto> GenerateJwtToken(IdentityUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Busca os perfis (roles) do usuário
            var userRoles = await _userManager.GetRolesAsync(user);

            // Adiciona Ccada perfil como uma "Claim" (informação) no token
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            
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