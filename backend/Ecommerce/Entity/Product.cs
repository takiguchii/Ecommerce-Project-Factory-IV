namespace Ecommerce.Entity;

public class Product
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public decimal OriginalPrice { get; set; } 
    public decimal? DiscountPrice { get; set; } // '?' significa que pode ser nulo
    public string Description { get; set; }
    public string TechnicalInfo { get; set; }
    public string RawDescription { get; set; }
    public string RawTechnicalInfo { get; set; }
    public int Rating { get; set; } // int para números de avaliação
    public int RatingQuantity { get; set; }

    // Chaves Estrangeiras ( não vou fazer 
    //public int Category { get; set; }
    //public int SubCategory { get; set; }
    //public int Provider { get; set; }
}