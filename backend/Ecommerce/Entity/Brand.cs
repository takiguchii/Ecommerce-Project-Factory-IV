using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity;

public class Brand
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public ICollection<Product> Products { get; set; }
}