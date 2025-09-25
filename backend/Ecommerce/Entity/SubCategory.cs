using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity;

public class SubCategory
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(128)]
    public string Name { get; set; }
    public int ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }
    public ICollection<Product> Products { get; set; }
}