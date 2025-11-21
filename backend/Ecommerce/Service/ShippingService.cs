using System.Text.Json;
using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service;

public class ShippingService : IShippingService
{
    private readonly HttpClient _httpClient;

    public ShippingService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<List<ShippingResultDto>> CalculateShippingAsync(CreateCalculateShippingDto model)
    {
        var cleanCep = model.Cep.Replace("-", "").Trim();
        
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cleanCep}/json/");
        
        if (!response.IsSuccessStatusCode) return new List<ShippingResultDto>();

        var jsonString = await response.Content.ReadAsStringAsync();
        
        using var document = JsonDocument.Parse(jsonString);
        var root = document.RootElement;

        if (root.TryGetProperty("erro", out _)) return new List<ShippingResultDto>();

        string uf = root.GetProperty("uf").GetString();

        decimal basePrice = GetBasePriceByState(uf);

        var options = new List<ShippingResultDto>
        {
            new ShippingResultDto 
            { 
                Name = "Jamef", 
                Carrier = "Transportadora Padrão",
                Price = basePrice, 
                EstimatedDeliveryDays = 5 
            },
            new ShippingResultDto 
            { 
                Name = "Jadlog", 
                Carrier = "Transportadora Rapido",
                Price = basePrice * 1.8m, 
                EstimatedDeliveryDays = 3 
            },
            new ShippingResultDto 
            { 
                Name = "Sedex", 
                Carrier = "Correios",
                Price = basePrice * 1.2m, 
                EstimatedDeliveryDays = 9 
            }
        };

        return options;
    }

    private decimal GetBasePriceByState(string uf)
    {
        return uf switch
        {
            "SP" => 08.50m, 
            "RJ" => 12.00m,
            "MG" => 13.00m,
            "PR" => 12.00m,
            "SC" => 18.00m,
            "RS" => 25.00m,
            "BA" or "PE" or "CE" => 25.00m,
            "AM" or "RR" or "AC" => 30.00m,
            _ => 35.00m 
        };
    }
}