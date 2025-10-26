using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Ecommerce.Entity;

public class Provider
{
    [Key]
    public int id { get; set; }

    [Required]
    [StringLength(32)]
    public string name { get; set; }

    [Required]
    [StringLength(16)]
    public string cnpj { get; set; }

    [Required]
    [StringLength(64)]
    public string email { get; set; }

    [Required]
    [StringLength(16)]
    public string phone_number { get; set; }

    [Required]
    [StringLength(128)]
    public string address { get; set; } 

    public ICollection<Product> Products { get; set; }
}