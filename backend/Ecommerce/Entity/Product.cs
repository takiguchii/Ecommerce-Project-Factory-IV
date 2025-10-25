using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Entity
{

    public class Product
    {

        public int id { get; set; } 

        [Required]
        [StringLength(16)] 
        public string code { get; set; }

        [Required]
        [StringLength(128)] 
        public string name { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10, 2)")] 
        public decimal original_price { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal discount_price { get; set; }

        [Required]
        [Column(TypeName = "TEXT")]
        public string description { get; set; }

        [Required]
        [Column(TypeName = "TEXT")]
        public string technical_info { get; set; }
        
        [StringLength(512)]
        public string? image_url0 { get; set; } 

        [StringLength(512)]
        public string? image_url1 { get; set; } 

        [StringLength(512)]
        public string? image_url2 { get; set; } 

        [StringLength(512)]
        public string? image_url3 { get; set; } 

        [StringLength(512)]
        public string? image_url4 { get; set; } 
        
        [Required] 
        [Column(TypeName = "decimal(3, 2)")] 
        public decimal stars { get; set; } 

        [Required] 
        public int rating { get; set; } 

        public int category_id { get; set; }

        public int? sub_category_id { get; set; } 

        public int brand_id { get; set; }

        public int provider_id { get; set; }

        public virtual Category Category { get; set; }
        public virtual SubCategory? SubCategory { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Provider Provider { get; set; }


    }
}