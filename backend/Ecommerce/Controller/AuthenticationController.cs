using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services; // Importa a nova interface
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // O Controller agora só conhece a INTERFACE do serviço
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        // --- ENDPOINT DE LOGIN (LIMPO) ---
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                // 1. Tenta fazer o login
                var loginResponse = await _authService.LoginAsync(dto);
                return Ok(loginResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                // 2. Captura a exceção de falha (senha/usuário errados)
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // 3. Captura qualquer outro erro inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Ocorreu um erro interno.", details = ex.Message });
            }
        }

        // --- ENDPOINT DE REGISTRO (LIMPO) ---
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                // 1. Tenta registrar
                var result = await _authService.RegisterAsync(dto);

                if (result.Succeeded)
                {
                    return Ok(new { message = "Usuário criado com sucesso!" });
                }

                // 2. Se a falha for conhecida (email/usuário em uso, senha fraca)
                return BadRequest(new { message = "Falha ao criar usuário.", errors = result.Errors });
            }
            catch (Exception ex)
            {
                // 3. Captura qualquer outro erro inesperado
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = "Ocorreu um erro interno.", details = ex.Message });
            }
        }
    }
}