using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class AddToCartDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "A quantidade deve ser pelo menos 1.")]
        public int Quantity { get; set; }
    }
}