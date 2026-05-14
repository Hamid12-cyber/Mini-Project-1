using Mini_Project_1.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Project_1.Models
{
    internal class Order
    {

        private static int _idCounter = 1;
        private string _email;
        public int Id { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal Total
        {
            get
            {
                decimal sum = 0;

                foreach (var item in Items)
                {
                    sum += item.SubTotal;
                }

                return sum;
            }

        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (!value.Contains('@')) throw new ArgumentException($"Yanlış format. Emaildə @ işarəsi vacibdir. ");
                _email = value;
            }
        }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderedAt { get; set; }
        public Order() { }
        public Order(string email, List<OrderItem> items)
        {
            Id = _idCounter++;
            Email = email;
            Items = items;
            Status = OrderStatus.Pending;
            OrderedAt = DateTime.Now;
        }
        public static void SyncCounter(int maxExistingId)
        {
            if (maxExistingId >= _idCounter)
                _idCounter = maxExistingId + 1;
        }
        public void PrintInfo()
        {
            Console.WriteLine($"  Sifariş nömrəsi  : {Id}");
            Console.WriteLine($"  Email     : {Email}");
            Console.WriteLine($"  Statusu    : {Status}");
            Console.WriteLine($"  Sifariş tarixi: {OrderedAt}");
            Console.WriteLine($"  Yekun mebleğ     : {Total:C}");
            foreach (var item in Items)
            {
                Console.WriteLine($"    [{item.Id}] {item.Product.Name} x{item.Count}" +
                                  $"  @{item.Price:C}  => {item.SubTotal:Azn}");
            }
        }
    }
}