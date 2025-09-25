using Microsoft.AspNetCore.Mvc;
using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Controller;

[ApiController]
[Route("api/providers")]
public class ProviderController : ControllerBase
{
    private readonly IProviderService _providerService;

    public ProviderController(IProviderService providerService)
    {
        _providerService = providerService;
    }

    [HttpPost]
    public IActionResult CreateProvider([FromBody] CreateProviderDto providerDto)
    {
        var result = _providerService.CreateProvider(providerDto);
        
        if (result is Provider provider)
        {
            return CreatedAtAction(nameof(GetProviderById), new { id = provider.Id }, provider);
        }
        
        return BadRequest(result);
    }

    [HttpGet]
    public IActionResult GetAllProviders()
    {
        var providers = _providerService.GetAllProviders();
        return Ok(providers);
    }

    [HttpGet("{id}")]
    public IActionResult GetProviderById(int id)
    {
        var provider = _providerService.GetProviderById(id);
        if (provider == null)
        {
            return NotFound();
        }
        return Ok(provider);
    }
}