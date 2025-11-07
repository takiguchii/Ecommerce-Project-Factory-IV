using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class CreateUpdateCouponDto
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public decimal DiscountValue { get; set; }

        [Required]
        public bool IsPercentage { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}