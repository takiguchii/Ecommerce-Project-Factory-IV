using System.Collections.Generic;

namespace Ecommerce.DTOs
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal TotalValue { get; set; }
    }
}