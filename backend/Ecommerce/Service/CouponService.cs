using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<Coupon?> GetCouponById(Guid id)
        {
            return await _couponRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Coupon>> GetAllCoupons()
        {
            return await _couponRepository.GetAllAsync();
        }

        public async Task<Coupon> CreateCoupon(CreateCouponDto createDto)
        {
            if (await _couponRepository.CodeExistsAsync(createDto.Code))
            {
                throw new Exception("O código do cupom já existe."); 
            }
            
            if (createDto.ExpiryDate <= DateTime.UtcNow)
            {
                 throw new Exception("A data de expiração deve ser no futuro.");
            }

            var coupon = new Coupon
            {
                Id = Guid.NewGuid(),
                Code = createDto.Code.ToUpper(), 
                DiscountValue = createDto.DiscountValue,
                IsPercentage = createDto.IsPercentage,
                ExpiryDate = createDto.ExpiryDate,
                IsActive = createDto.IsActive
            };

            await _couponRepository.AddAsync(coupon);
            return coupon;
        }

        public async Task<Coupon?> UpdateCoupon(Guid id, CreateUpdateCouponDto updateDto)
        {
            var existingCoupon = await _couponRepository.GetByIdAsync(id);
            if (existingCoupon == null)
            {
                return null; 
            }

            if (existingCoupon.Code.ToUpper() != updateDto.Code.ToUpper())
            {
                if (await _couponRepository.CodeExistsAsync(updateDto.Code))
                {
                    throw new Exception("O novo código de cupom já está em uso.");
                }
            }

            existingCoupon.Code = updateDto.Code.ToUpper();
            existingCoupon.DiscountValue = updateDto.DiscountValue;
            existingCoupon.IsPercentage = updateDto.IsPercentage;
            existingCoupon.ExpiryDate = updateDto.ExpiryDate;
            existingCoupon.IsActive = updateDto.IsActive;

            await _couponRepository.UpdateAsync(existingCoupon);
            return existingCoupon;
        }

        public async Task<bool> DeleteCoupon(Guid id)
        {
            var existingCoupon = await _couponRepository.GetByIdAsync(id);
            if (existingCoupon == null)
            {
                return false; 
            }

            await _couponRepository.DeleteAsync(id);
            return true; 
        }
        
        public async Task<Coupon?> ValidateCoupon(string code)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            
            if (coupon == null || !coupon.IsActive || coupon.ExpiryDate <= DateTime.UtcNow)
            {
                return null; 
            }

            return coupon; 
        }
    }
}