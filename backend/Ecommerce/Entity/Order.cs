using System;
using System.Collections.Generic;

namespace Ecommerce.Entity
{
    public class Order
    {
        public int Id { get; set; }
        
        public int UserId { get; set; } 
        
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public decimal SubTotal { get; set; } 
        public decimal Total { get; set; }    

        // filtros do futuri 
        //public decimal ShippingCost { get; set; } = 0; // Custo do Frete
        //public decimal Discount { get; set; } = 0;     // Desconto do Cupom
        //public string? ShippingAddress { get; set; }  // Endereço de entrega
        //public string? PaymentMethod { get; set; }    // "Pix" ou "Cartao"

        public string Status { get; set; } = "Aguardando Pagamento";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}