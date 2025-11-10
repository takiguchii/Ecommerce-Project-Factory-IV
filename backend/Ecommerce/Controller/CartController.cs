using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize] 
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetCurrentAppUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var appUserId = GetCurrentAppUserId();
            var cart = await _cartService.GetCartAsync(appUserId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToMyCart([FromBody] AddToCartDto dto)
        {
            var appUserId = GetCurrentAppUserId();
            var cart = await _cartService.AddToCartAsync(appUserId, dto);
            return Ok(cart);
        }
        
        [HttpPut("update/{productId}")]
        public async Task<IActionResult> UpdateItemQuantity(int productId, [FromQuery] int quantity)
        {
            var appUserId = GetCurrentAppUserId();
            
            if (quantity <= 0)
            {
                var cart = await _cartService.RemoveFromCartAsync(appUserId, productId);
                return Ok(cart);
            }
            
            var updatedCart = await _cartService.UpdateItemQuantityAsync(appUserId, productId, quantity);
            return Ok(updatedCart);
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromMyCart(int productId)
        {
            var appUserId = GetCurrentAppUserId();
            var cart = await _cartService.RemoveFromCartAsync(appUserId, productId);
            return Ok(cart);
        }
    }
}