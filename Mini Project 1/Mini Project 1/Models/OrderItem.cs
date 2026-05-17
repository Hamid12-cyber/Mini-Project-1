using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Models
{
    internal class OrderItem
    {           
             
        public Guid Id { get;  }

        public Product Product { get; set; }
        public string DeliveryType { get; set; }
        public decimal DeliveryFee { get; set; }
        public int Count { get; set; }
             
        public decimal Price { get; set; }

       public decimal SubTotal => Price * Count;
             
        public OrderItem() { }
        public OrderItem(Product product, int count)
        {
            Id = Guid.NewGuid();
            Product = product;            
            Count = count;
            Price = product.Price; 
        }
       
    }
}
