using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon([FromBody] CreateCouponDto couponDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try {
                var newCoupon = await _couponService.CreateCoupon(couponDto);
                return CreatedAtAction(nameof(GetCouponById), new { id = newCoupon.Id }, newCoupon);
            }
            catch (Exception ex) 
            {
                return BadRequest(new { message = ex.Message });
            } ;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoupons()
        {
            var coupons = await _couponService.GetAllCoupons();
            return Ok(coupons);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCouponById(Guid id)
        {
            var coupon = await _couponService.GetCouponById(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return Ok(coupon);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCoupon(Guid id, [FromBody] CreateUpdateCouponDto couponDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCoupon = await _couponService.UpdateCoupon(id, couponDto);
            if (updatedCoupon == null)
            {
                return NotFound();
            }
            return Ok(updatedCoupon);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCoupon(Guid id)
        {
            var result = await _couponService.DeleteCoupon(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent(); 
        }
        
        [HttpGet("validate/{code}")]
        public async Task<IActionResult> ValidateCoupon(string code)
        {
            var coupon = await _couponService.ValidateCoupon(code);
            if (coupon == null)
            {
                return NotFound(new { message = "Cupom inválido ou expirado." });
            }
            return Ok(coupon);
        }
    }
}