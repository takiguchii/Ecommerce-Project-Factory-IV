using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity
{
    public class SubCategory
    {
        public int id { get; set; } 
        
        [Required]
        [StringLength(32)]
        public string name { get; set; } 
        public int category_id { get; set; } 
        
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}



