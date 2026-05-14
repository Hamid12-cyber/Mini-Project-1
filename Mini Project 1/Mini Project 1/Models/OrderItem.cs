using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Models
{
    internal class OrderItem
    {

        private static int _idCounter = 1;
               
        public int Id { get; set; }

        public Product Product { get; set; }

        public int Count { get; set; }
             
        public decimal Price { get; set; }

       public decimal SubTotal => Price * Count;
             
        public OrderItem() { }
        public OrderItem(Product product, int count)
        {
            Id = _idCounter++;
            Product= product;
            Count = count;
            Price = product.Price; 
        }
        public static void SyncCounter(int maxExistingId)
        {
            if (maxExistingId >= _idCounter)
                _idCounter = maxExistingId + 1;
        } 
    }
}
