using System.ComponentModel.DataAnnotations; 

namespace Ecommerce.Dto;

    public class CreateProductDto
    {

        [Required(ErrorMessage = "Código é obrigatório.")]
       
        public string code { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Preço original é obrigatório.")]
        public string original_price { get; set; }

        [Required(ErrorMessage = "Preço com desconto é obrigatório.")]
        public string discount_price { get; set; } 

        [Required(ErrorMessage = "Descrição é obrigatória.")]
        public string description { get; set; }

        [Required(ErrorMessage = "Informação técnica é obrigatória.")]
        public string technical_info { get; set; }

        public string? image_url0 { get; set; }
        public string? image_url1 { get; set; }
        public string? image_url2 { get; set; }
        public string? image_url3 { get; set; }
        public string? image_url4 { get; set; }


        [Required(ErrorMessage = "ID da Categoria é obrigatório.")]
        public int category_id { get; set; }

        public int? sub_category_id { get; set; } 

        [Required(ErrorMessage = "ID da Marca é obrigatório.")]
        public int brand_id { get; set; }

        [Required(ErrorMessage = "ID do Fornecedor é obrigatório.")]
        public int provider_id { get; set; }

    }
