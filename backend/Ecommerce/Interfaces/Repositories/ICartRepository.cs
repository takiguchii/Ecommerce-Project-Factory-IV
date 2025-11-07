using Ecommerce.Entity;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetByUserIdAsync(string appUserId);
        
        Task<CartItem> GetItemInCartAsync(int cartId, int productId);

        void Add(Cart cart);
        void AddItem(CartItem cartItem);
        void UpdateItem(CartItem cartItem);
        void RemoveItem(CartItem cartItem);
        
        Task SaveChangesAsync();
    }
}