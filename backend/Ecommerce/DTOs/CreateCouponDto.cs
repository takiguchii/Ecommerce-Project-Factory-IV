using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class CreateCouponDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public bool IsPercentage { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true; 
    }
}