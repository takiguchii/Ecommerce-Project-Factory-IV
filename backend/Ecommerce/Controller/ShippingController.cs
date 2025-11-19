using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller;

[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
    private readonly IShippingService _shippingService;

    public ShippingController(IShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] CreateCalculateShippingDto model)
    {
        if (string.IsNullOrEmpty(model.Cep))
            return BadRequest("CEP é obrigatório");

        var result = await _shippingService.CalculateShippingAsync(model);
        return Ok(result);
    }
}