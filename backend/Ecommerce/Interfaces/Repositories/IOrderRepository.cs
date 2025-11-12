using Ecommerce.Entity;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        // Task<Order?> GetByIdAsync(int id);
        // Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    }
}