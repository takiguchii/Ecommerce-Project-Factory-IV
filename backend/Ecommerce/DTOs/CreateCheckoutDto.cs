using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class CreateCheckoutDto
    {
        [Required]
        public string PaymentMethod { get; set; }
    }
}