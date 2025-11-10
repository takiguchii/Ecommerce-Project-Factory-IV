using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly EcommerceDbContext _dbContext;

        public CartRepository(EcommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Cart cart)
        {
            _dbContext.Carts.Add(cart);
        }

        public void AddItem(CartItem cartItem)
        {
            _dbContext.CartItems.Add(cartItem);
        }

        public async Task<Cart> GetByUserIdAsync(string appUserId)
        {
            return await _dbContext.Carts
                .Include(c => c.Items)
                .ThenInclude(item => item.Product) 
                .FirstOrDefaultAsync(c => c.AppUserId == appUserId);
        }

        public async Task<CartItem> GetItemInCartAsync(int cartId, int productId)
        {
            return await _dbContext.CartItems
                .FirstOrDefaultAsync(item => item.CartId == cartId && item.ProductId == productId);
        }

        public void RemoveItem(CartItem cartItem)
        {
            _dbContext.CartItems.Remove(cartItem);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void UpdateItem(CartItem cartItem)
        {
            _dbContext.Entry(cartItem).State = EntityState.Modified;
        }
    }
}