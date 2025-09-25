using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs;

public class CreateSubCategoryDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    public int ParentCategoryId { get; set; }
}