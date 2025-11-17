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


        public async Task<Order> CreateOrderFromCartAsync(string appUserId, int numericUserId, CreateCheckoutDto dto) 
        {
            if (string.IsNullOrWhiteSpace(dto.PaymentMethod))
            {
                throw new InvalidOperationException("O método de pagamento é obrigatório.");
            }

            string paymentMethod = dto.PaymentMethod.ToUpperInvariant();

            if (paymentMethod != "PIX" && paymentMethod != "CREDITCARD") 
            {
                throw new InvalidOperationException("Método de pagamento inválido. Aceito apenas 'PIX' ou 'CREDITCARD'.");
            }

            var cartDto = await _cartService.GetCartAsync(appUserId);

            if (cartDto == null || !cartDto.Items.Any())
            {
                throw new InvalidOperationException("Não é possível criar um pedido de um carrinho vazio.");
            }

            var order = new Order
            {
                UserId = numericUserId,
                Status = "Aguardando Pagamento",
                CreatedAt = DateTime.UtcNow,
                PaymentMethod = paymentMethod 
            };

            foreach (var itemDto in cartDto.Items)
            {
                decimal price;
                try
                {
                    string sanitizedPrice = itemDto.Price
                        .Replace("R$", "").Replace(".", "").Replace(",", ".").Trim();
                    price = decimal.Parse(sanitizedPrice, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    throw new FormatException($"Formato de preço inválido no DTO: {itemDto.Price}", ex);
                }

                order.Items.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    Price = price,
                    ProductName = itemDto.Name,
                    ImageUrl = itemDto.ImageUrl
                });
            }

            order.SubTotal = cartDto.TotalValue;
            order.Total = cartDto.TotalValue;

            await _orderRepository.AddAsync(order);
            await _cartService.ClearCartAsync(appUserId);

            return order;
        }
    }
}