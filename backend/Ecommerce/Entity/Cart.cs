using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}