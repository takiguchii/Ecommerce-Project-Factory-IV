using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetByIdAsync(Guid id);
        Task<Coupon?> GetByCodeAsync(string code); 
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task AddAsync(Coupon coupon);
        Task UpdateAsync(Coupon coupon);
        Task DeleteAsync(Guid id);
        Task<bool> CodeExistsAsync(string code); 
    }
}