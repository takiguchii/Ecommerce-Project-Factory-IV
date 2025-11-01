using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize] // Este controller SÓ funciona para usuários logados
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Pega o ID (GUID) do usuário que está no Token JWT
        private string GetCurrentAppUserId()
        {
            // O NameIdentifier é o 'user.Id' que vamos adicionar ao token
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var appUserId = GetCurrentAppUserId();
            if (string.IsNullOrEmpty(appUserId))
            {
                return Unauthorized("Não foi possível identificar o usuário.");
            }

            var profile = await _userService.GetMyProfileAsync(appUserId);
            if (profile == null)
            {
                return NotFound("Perfil de usuário não encontrado.");
            }
            return Ok(profile);
        }
        
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateUserDto dto)
        {
            var appUserId = GetCurrentAppUserId();
            if (string.IsNullOrEmpty(appUserId))
            {
                return Unauthorized("Não foi possível identificar o usuário.");
            }

            var updatedProfile = await _userService.UpdateMyProfileAsync(appUserId, dto);
            if (updatedProfile == null)
            {
                return NotFound("Perfil de usuário não encontrado.");
            }
            return Ok(updatedProfile);
        }
    }
}