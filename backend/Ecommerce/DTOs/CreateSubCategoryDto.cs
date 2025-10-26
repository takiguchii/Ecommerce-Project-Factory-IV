using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class CreateSubCategoryDto
    {
        [Required(ErrorMessage = "O nome da subcategoria é obrigatório.")]
        public string name { get; set; }

        [Required(ErrorMessage = "O ID da categoria pai é obrigatório.")]
        public int category_id { get; set; } 
    }
}