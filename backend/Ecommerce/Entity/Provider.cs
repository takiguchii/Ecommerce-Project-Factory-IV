using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity;

public class Provider
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(256)]
    public string Name { get; set; }
    [Required]
    [StringLength(14)]
    public string Cnpj { get; set; }
    public ICollection<Product> Products { get; set; }
}