using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; }
    public ICollection<Product> Products { get; set; }
}