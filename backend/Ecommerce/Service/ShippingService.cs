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
                Name = "Econômico", 
                Carrier = "Transportadora Padrão",
                Price = basePrice, 
                EstimatedDeliveryDays = 7 
            },
            new ShippingResultDto 
            { 
                Name = "Rápido", 
                Carrier = "Flash Courier",
                Price = basePrice * 1.5m, 
                EstimatedDeliveryDays = 3 
            },
            new ShippingResultDto 
            { 
                Name = "Sedex", 
                Carrier = "Correios",
                Price = basePrice * 2.0m, 
                EstimatedDeliveryDays = 1 
            }
        };

        return options;
    }

    private decimal GetBasePriceByState(string uf)
    {
        return uf switch
        {
            "SP" => 15.00m, 
            "RJ" => 20.00m,
            "MG" => 20.00m,
            "PR" => 22.00m,
            "SC" => 25.00m,
            "RS" => 25.00m,
            "BA" or "PE" or "CE" => 45.00m,
            "AM" or "RR" or "AC" => 60.00m,
            _ => 35.00m // Preço padrão para o resto do Brasil
        };
    }
}