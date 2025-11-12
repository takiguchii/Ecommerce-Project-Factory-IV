using Ecommerce.Entity;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromCartAsync(string appUserId, int numericUserId);
    }
}