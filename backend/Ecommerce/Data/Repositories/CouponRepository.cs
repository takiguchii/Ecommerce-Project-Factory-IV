using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly EcommerceDbContext _context;

        public CouponRepository(EcommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Coupon?> GetByIdAsync(Guid id)
        {
            return await _context.Coupons.FindAsync(id);
        }

        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            return await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code.ToUpper() == code.ToUpper());
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _context.Coupons.AsNoTracking().ToListAsync();
        }

        public async Task AddAsync(Coupon coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var coupon = await GetByIdAsync(id);
            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<bool> CodeExistsAsync(string code)
        {
            return await _context.Coupons
                .AnyAsync(c => c.Code.ToUpper() == code.ToUpper());
        }
    }
}