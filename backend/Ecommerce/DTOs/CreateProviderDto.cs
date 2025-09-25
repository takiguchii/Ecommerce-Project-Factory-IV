using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs;

public class CreateProviderDto
{
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
}