using Ecommerce.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Interfaces.Services
{
    public interface IAuthenticationService
    {
        // Contrato para o Login, que retorna o DTO com o token
        Task<LoginResponseDto> LoginAsync(LoginDto dto);

        // Contrato para o Registro. Retornará o resultado do Identity
        Task<IdentityResult> RegisterAsync(RegisterDto dto);
    }
}