using Ecommerce.DTOs; 
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;

        public OrderService(IOrderRepository orderRepository, ICartService cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
        }

        public async Task<Order> CreateOrderFromCartAsync(string appUserId, int numericUserId)
        {
            var cartDto = await _cartService.GetCartAsync(appUserId);

            if (cartDto == null || !cartDto.Items.Any())
            {
                throw new InvalidOperationException("Não é possível criar um pedido de um carrinho vazio.");
            }

            var order = new Order
            {
                UserId = numericUserId, 
                Status = "Aguardando Pagamento", 
                CreatedAt = DateTime.UtcNow
            };

            foreach (var itemDto in cartDto.Items)
            {
                decimal price;
                try
                {
                    string rawPrice = itemDto.Price;
                    string sanitizedPrice = rawPrice
                        .Replace("R$", "")    // Remove "R$"
                        .Replace(".", "")     // Remove o ponto de milhar
                        .Replace(",", ".")    // Troca a vírgula de decimal por ponto
                        .Trim();              // Remove espaços

                    // 3. Converte a string limpa (ex: "2314.90")
                    price = decimal.Parse(sanitizedPrice, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Formato de preço inválido no DTO: {itemDto.Price}", ex);
                }

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = price,
                    ProductName = itemDto.Name,
                    ImageUrl = itemDto.ImageUrl
                };
                order.Items.Add(orderItem);
            }
            
            order.SubTotal = cartDto.TotalValue;
            order.Total = cartDto.TotalValue; 

            await _orderRepository.AddAsync(order);
            await _cartService.ClearCartAsync(appUserId);

            return order;
        }
    }
}