using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Entity;

public class Product
{
    [Key]
    public int Id { get; set; } 
    public string Name { get; set; }
    public decimal OriginalPrice { get; set; } 
    public decimal? DiscountPrice { get; set; }
    public string Description { get; set; }
    public string TechnicalInfo { get; set; }
    public string RawDescription { get; set; }
    public string RawTechnicalInfo { get; set; }
    public int Rating { get; set; }
    public int RatingQuantity { get; set; }

    //Adicionando as ForeKey com suas relações 
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int SubCategoryId { get; set; }
    public SubCategory SubCategory { get; set; }
    public int ProviderId { get; set; }
    public Provider Provider { get; set; }
}