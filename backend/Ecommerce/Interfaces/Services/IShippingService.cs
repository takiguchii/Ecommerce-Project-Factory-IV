using Ecommerce.DTOs;

namespace Ecommerce.Interfaces.Services;

public interface IShippingService
{
    Task<List<ShippingResultDto>> CalculateShippingAsync(CreateCalculateShippingDto model);
}