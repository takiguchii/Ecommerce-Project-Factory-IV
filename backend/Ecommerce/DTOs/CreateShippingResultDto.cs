namespace Ecommerce.DTOs;

public class ShippingResultDto
{
    public string Name { get; set; }            
    public decimal Price { get; set; }          
    public int EstimatedDeliveryDays { get; set; } 
    public string Carrier { get; set; }   
}