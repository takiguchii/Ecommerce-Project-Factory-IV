using Ecommerce.DTOs; 
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;
using System.Globalization;


namespace Ecommerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly ICouponRepository _couponRepository;

        public OrderService(IOrderRepository orderRepository, ICartService cartService,ICouponRepository couponRepository)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
            _couponRepository = couponRepository;
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
            throw new InvalidOperationException("Método de pagamento inválido.");
        }

        var cartDto = await _cartService.GetCartAsync(appUserId);

        if (cartDto == null || !cartDto.Items.Any())
        {
             throw new InvalidOperationException("Carrinho vazio.");
        }

        var order = new Order
        {
            UserId = numericUserId,
            Status = "Pagamento concluido", 
            CreatedAt = DateTime.UtcNow,
            PaymentMethod = paymentMethod, 
            Carrier = dto.Carrier,
            ShippingCost = dto.ShippingCost,
            CouponCode = dto.CouponCode 
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
                throw new FormatException($"Preço inválido: {itemDto.Price}", ex);
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
        
         decimal discountAmount = 0;

         if (!string.IsNullOrEmpty(dto.CouponCode))
        {
            var coupon = await _couponRepository.GetByCodeAsync(dto.CouponCode);
        
            if (coupon != null && coupon.IsActive && coupon.ExpiryDate >= DateTime.UtcNow)
            {
            if (coupon.IsPercentage)
                {
                    discountAmount = cartDto.TotalValue * (coupon.DiscountValue / 100m);
                }
                else
                {
                    discountAmount = coupon.DiscountValue;
                }
            }
        }

        order.Discount = discountAmount;

        order.SubTotal = cartDto.TotalValue; 
        
        order.Total = Math.Max(0, order.SubTotal + dto.ShippingCost - discountAmount);

        await _orderRepository.AddAsync(order);
        await _cartService.ClearCartAsync(appUserId);

        return order;
        }
    }
}