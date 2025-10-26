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
        var existingProvider = _providerRepository.GetByCnpj(providerDto.cnpj);
        if (existingProvider != null)
        {
            return new { Message = "Já existe um fornecedor cadastrado com este CNPJ." };
        }

        var newProvider = new Provider
        {
            name = providerDto.name,
            cnpj = providerDto.cnpj,
            email = providerDto.email,
            phone_number = providerDto.phone_number,
            address = providerDto.address 
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
    public Provider? UpdateProvider(int id, CreateProviderDto providerDto)
    {
        var existingProvider = _providerRepository.GetById(id);
        if (existingProvider == null)
        {
            return null;
        }

        existingProvider.name = providerDto.name;
        existingProvider.cnpj = providerDto.cnpj;
        existingProvider.email = providerDto.email;
        existingProvider.phone_number = providerDto.phone_number;
        existingProvider.address = providerDto.address;
    
        _providerRepository.Update(existingProvider);
        _providerRepository.SaveChanges();

        // 5. Retorna a entidade atualizada
        return existingProvider;
    }
    public bool DeleteProvider(int id)
    {
        var provider = _providerRepository.GetById(id);

        if (provider == null)
        {
            return false;
        }

        _providerRepository.Delete(provider);
        _providerRepository.SaveChanges();

        return true;
    }
}