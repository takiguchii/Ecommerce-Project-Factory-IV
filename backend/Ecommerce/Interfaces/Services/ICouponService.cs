using Ecommerce.DTOs;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services
{
    public interface ICouponService
    {
        Task<Coupon?> GetCouponById(Guid id);
        Task<IEnumerable<Coupon>> GetAllCoupons();
        Task<Coupon> CreateCoupon(CreateCouponDto createDto);
        Task<Coupon?> UpdateCoupon(Guid id, CreateUpdateCouponDto updateDto);
        Task<bool> DeleteCoupon(Guid id);
        Task<Coupon?> ValidateCoupon(string code); 
    }
}