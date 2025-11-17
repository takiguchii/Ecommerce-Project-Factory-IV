using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Ecommerce.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EcommerceDbContext _context;

        public OrderRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}