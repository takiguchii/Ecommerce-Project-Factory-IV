using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
    public string name { get; set; }
}