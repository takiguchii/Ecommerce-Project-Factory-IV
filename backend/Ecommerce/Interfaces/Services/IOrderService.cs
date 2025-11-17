using Ecommerce.Entity;
using System.Threading.Tasks;
using Ecommerce.DTOs;

namespace Ecommerce.Interfaces.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderFromCartAsync(string appUserId, int numericUserId, CreateCheckoutDto dto);
        
    }
}