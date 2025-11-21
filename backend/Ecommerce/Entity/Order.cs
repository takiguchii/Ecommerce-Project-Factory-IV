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
        public decimal ShippingCost { get; set; } = 0; 
        public string? Carrier { get; set; } 
        //public decimal Discount { get; set; } = 0;     
        //public string? ShippingAddress { get; set; } 
        public string? PaymentMethod { get; set; }   
        public string Status { get; set; } = "Aguardando Pagamento";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}