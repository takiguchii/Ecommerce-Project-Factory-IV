namespace Ecommerce.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Price { get; set; } 
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } 
    }
}