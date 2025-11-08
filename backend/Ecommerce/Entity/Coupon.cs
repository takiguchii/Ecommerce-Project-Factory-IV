using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity
{
    public class Coupon
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; } 
        [MaxLength(255)]
        public string? Description { get; set; }

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