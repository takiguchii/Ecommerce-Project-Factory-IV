using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

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

    [Required]
    [StringLength(64)]
    public string Email { get; set; }

    [Required]
    [StringLength(11)]
    public string PhoneNumber { get; set; }

    [Required]
    [StringLength(256)]
    public string Address { get; set; } 

    public ICollection<Product> Products { get; set; }
}