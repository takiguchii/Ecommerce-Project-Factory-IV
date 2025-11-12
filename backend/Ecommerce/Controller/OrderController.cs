using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.Controller
{
    [ApiController]
    [Route("api/orders")]
    [Authorize] 
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserRepository _userRepository;

        public OrderController(IOrderService orderService, IUserRepository userRepository)
        {
            _orderService = orderService;
            _userRepository = userRepository;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            try
            {
                var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(appUserId))
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                var user = await _userRepository.FindByAppUserIdAsync(appUserId);
                if (user == null)
                {
                    return Unauthorized("Usuário não encontrado no sistema.");
                }

                var numericUserId = user.Id;

                var order = await _orderService.CreateOrderFromCartAsync(appUserId, numericUserId);

                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (FormatException ex)
            {
                return BadRequest(new { message = $"Erro no formato dos dados: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erro interno no servidor: {ex.Message}" });
            }
        }
    }
}