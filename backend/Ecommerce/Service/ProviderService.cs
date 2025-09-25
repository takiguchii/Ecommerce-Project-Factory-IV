using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service;

public class ProviderService : IProviderService
{
    private readonly IProviderRepository _providerRepository;

    public ProviderService(IProviderRepository providerRepository)
    {
        _providerRepository = providerRepository;
    }

    public object? CreateProvider(CreateProviderDto providerDto)
    {
        // Regra de Negócio: Verificar se já existe um fornecedor com o mesmo CNPJ.
        var existingProvider = _providerRepository.GetByCnpj(providerDto.Cnpj);
        if (existingProvider != null)
        {
            return new { Message = "Já existe um fornecedor cadastrado com este CNPJ." };
        }

        var newProvider = new Provider
        {
            Name = providerDto.Name,
            Cnpj = providerDto.Cnpj,
            Email = providerDto.Email,
            PhoneNumber = providerDto.PhoneNumber,
            Address = providerDto.Address 
        };

        _providerRepository.Add(newProvider);
        _providerRepository.SaveChanges();
        return newProvider;
    }
    
    public List<Provider> GetAllProviders()
    {
        return _providerRepository.GetAll();
    }

    public Provider? GetProviderById(int id)
    {
        return _providerRepository.GetById(id);
    }
}