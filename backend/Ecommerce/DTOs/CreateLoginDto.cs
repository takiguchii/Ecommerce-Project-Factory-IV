using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}