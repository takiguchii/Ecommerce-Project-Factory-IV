using Ecommerce.DTOs;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services;

public interface IProviderService
{
    object? CreateProvider(CreateProviderDto providerDto);
    List<Provider> GetAllProviders();
    Provider? GetProviderById(int id);
    bool DeleteProvider(int id);
}