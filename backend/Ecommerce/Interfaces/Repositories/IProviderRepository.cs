using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories;

public interface IProviderRepository
{
    void Add(Provider provider);
    List<Provider> GetAll();
    Provider? GetById(int id);
    Provider? GetByCnpj(string cnpj); // Método para verificar duplicadas
    void SaveChanges();
}