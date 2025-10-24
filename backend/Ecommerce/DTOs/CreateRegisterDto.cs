using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório")]
        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem")]
        public string? ConfirmPassword { get; set; }
    }
}