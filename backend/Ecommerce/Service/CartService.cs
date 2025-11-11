using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System; 

namespace Ecommerce.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository; 
        
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartDto> AddToCartAsync(string appUserId, AddToCartDto dto)
        {
            var productToAdd = _productRepository.GetById(dto.ProductId);
            if (productToAdd == null)
            {
                return await GetCartAsync(appUserId); 
            }

            var cart = await _cartRepository.GetByUserIdAsync(appUserId);

            if (cart == null)
            {
                cart = new Cart { AppUserId = appUserId };
                _cartRepository.Add(cart);
                await _cartRepository.SaveChangesAsync(); 
            }

            var existingItem = await _cartRepository.GetItemInCartAsync(cart.Id, dto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
                _cartRepository.UpdateItem(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };
                cart.Items.Add(newItem); 
            }
            
            await _cartRepository.SaveChangesAsync();
            return await GetCartAsync(appUserId);
        }

        public async Task<CartDto> GetCartAsync(string appUserId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(appUserId);
            return MapCartToDto(cart);
        }

        public async Task<CartDto> RemoveFromCartAsync(string appUserId, int productId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(appUserId);
            if (cart == null)
            {
                return new CartDto(); 
            }

            var itemToRemove = await _cartRepository.GetItemInCartAsync(cart.Id, productId);
            if (itemToRemove != null)
            {
                _cartRepository.RemoveItem(itemToRemove);
                await _cartRepository.SaveChangesAsync();
            }

            return await GetCartAsync(appUserId);
        }

        public async Task<CartDto> UpdateItemQuantityAsync(string appUserId, int productId, int newQuantity)
        {
            if (newQuantity <= 0)
            {
                return await RemoveFromCartAsync(appUserId, productId);
            }

            var cart = await _cartRepository.GetByUserIdAsync(appUserId);
            if (cart == null)
            {
                return new CartDto();
            }

            var itemToUpdate = await _cartRepository.GetItemInCartAsync(cart.Id, productId);

            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = newQuantity;
                _cartRepository.UpdateItem(itemToUpdate);
                await _cartRepository.SaveChangesAsync();
            }
            
            return await GetCartAsync(appUserId);
        }
        
        public async Task ClearCartAsync(string appUserId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(appUserId);

            if (cart != null && cart.Items.Any())
            {
                foreach (var item in cart.Items)
                {
                    _cartRepository.RemoveItem(item);
                }
                await _cartRepository.SaveChangesAsync();
            }
        }
        
        private CartDto MapCartToDto(Cart cart)
        {
            if (cart == null)
            {
                return new CartDto(); 
            }

            var cartDto = new CartDto();
            decimal total = 0;

            foreach (var item in cart.Items)
            {
                if (item.Product != null)
                {
                    var itemDto = new CartItemDto
                    {
                        ProductId = item.ProductId,
                        Name = item.Product.name, 
                        Price = item.Product.discount_price, 
                        Quantity = item.Quantity,
                        ImageUrl = item.Product.image_url0 
                    };
                    
                    cartDto.Items.Add(itemDto);

                    try
                    {
                        decimal price = decimal.Parse(item.Product.discount_price, 
                                        System.Globalization.CultureInfo.InvariantCulture);
                        total += (price * item.Quantity);
                    }
                    catch
                    {
                    }
                }
            }

            cartDto.TotalValue = total;
            return cartDto;
        }
    }
}