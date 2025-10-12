namespace Ecommerce.Dto;

public class CreateProductDto
{
    public string Name { get; set; }
    
    public string CoverImageUrl { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal? DiscountPrice { get; set; } 
    public string Description { get; set; }
    public string TechnicalInfo { get; set; }    
    public string RawDescription { get; set; }    
    public string RawTechnicalInfo { get; set; }  

    
}