namespace Ecommerce.Dto;

public class CreateProductDto
{
    public string Name { get; set; }
    public decimal OriginalPrice { get; set; }
    public string Description { get; set; }
    public int Category { get; set; }
    public int SubCategory { get; set; }
    public int Provider { get; set; }
}