using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTOs
{
    public class CreateProviderDto
    {
        [Required(ErrorMessage = "O nome do fornecedor é obrigatório.")]
        public string name { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        public string cnpj { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string email { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(16)] 
        public string phone_number { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        public string address { get; set; }
    }
}