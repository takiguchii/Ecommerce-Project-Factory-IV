using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; 
using System.Text.Json.Serialization; 

namespace Ecommerce.Entity
{
    public class Brand
    {
        public int id { get; set; }

        public string name { get; set; } 

        public string? brand_image_url { get; set; } 

        public virtual ICollection<Product> Products { get; set; } = new List<Product>(); 
    }
}