using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class CreateBrandDto
    {
        [Required(ErrorMessage = "O nome da marca é obrigatório.")]
        [StringLength(32)] 
        public string name { get; set; }

        [Required(ErrorMessage = "A URL da imagem da marca é obrigatória.")]
        [StringLength(512)] 
        public string brand_image_url { get; set; }
    }
}