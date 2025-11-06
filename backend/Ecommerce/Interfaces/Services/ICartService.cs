using Ecommerce.DTOs;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDto> AddToCartAsync(string appUserId, AddToCartDto dto);

        Task<CartDto> GetCartAsync(string appUserId);

        Task<CartDto> RemoveFromCartAsync(string appUserId, int productId);
        
        Task<CartDto> UpdateItemQuantityAsync(string appUserId, int productId, int newQuantity);
    }
}