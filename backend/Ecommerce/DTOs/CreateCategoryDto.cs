using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs;

public class CreateCategoryDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }
}