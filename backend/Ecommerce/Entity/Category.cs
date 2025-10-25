using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity;

public class Category
{
    [Key]
    public int id { get; set; }
    [Required]
    [StringLength(32)]
    public string name { get; set; }
    
    public ICollection<Product> Products { get; set; }
}