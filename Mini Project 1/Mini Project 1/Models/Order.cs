using Mini_Project_1.AbstractClasses;
using Mini_Project_1.Enums;

namespace Mini_Project_1.Models
{
    internal class Order : BaseEntity
    {
        private static int _idCounter = 1;
        private string _email;

        public int Id { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
                
        public decimal Total
        {
            get
            {
                decimal sum = Items.Sum(item => item.SubTotal);
                return sum + DeliveryFee;
            }
        }     
        public decimal DiscountPercent
        {
            get
            {
                decimal productSum = Items.Sum(item => item.SubTotal);
                if (productSum >= 300) return 10;
                if (productSum >= 100) return 5;
                return 0;
            }
        }
        public decimal Discount
        {
            get
            {
                decimal productSum = Items.Sum(item => item.SubTotal);
                return Math.Round(productSum * DiscountPercent / 100, 2);
            }
        }       
        public decimal FinalTotal => Total - Discount;

        public string Email
        {
            get { return _email; }
            set
            {
                if (!value.Contains('@'))
                    throw new ArgumentException("Yanlış format. Emaildə @ işarəsi vacibdir.");
                _email = value;
            }
        }

        public string DeliveryType { get; set; }
        public decimal DeliveryFee { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime OrderedAt { get; set; }

        public Order() { }

        public Order(string email, List<OrderItem> items, string deliveryType, decimal deliveryFee)
        {
            Id = _idCounter++;
            Email = email;
            DeliveryType = deliveryType;
            DeliveryFee = deliveryFee;
            Items = items;
            Status = OrderStatus.Pending;
            OrderedAt = DateTime.Now;
        }

        public static void SyncCounter(int maxExistingId)
        {
            if (maxExistingId >= _idCounter)
                _idCounter = maxExistingId + 1;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"  Sifariş nömrəsi : {Id}");
            Console.WriteLine($"  Email            : {Email}");
            Console.WriteLine($"  Statusu          : {Status}");
            Console.WriteLine($"  Sifariş tarixi   : {OrderedAt}");
            Console.WriteLine($"  Məhsul cəmi      : {Total - DeliveryFee} AZN");
            if (DiscountPercent > 0)
            {
                Console.WriteLine($"  Endirim ({DiscountPercent}%)   : -{Discount} AZN");
            }
            Console.WriteLine($"  Çatdırılma       : {DeliveryFee} AZN");
            Console.WriteLine($"  Yekun məbləğ     : {FinalTotal} AZN");
            foreach (var item in Items)
            {
                Console.WriteLine($"    - {item.Product.Name} x{item.Count}  @{item.Price} AZN  => {item.SubTotal} AZN");
            }
        }
        public void PrintReceipt()
        {
            Console.WriteLine("\n========= İnvoice =========");
            Console.WriteLine($"Order No   : #{Id}");
            Console.WriteLine($"Email      : {Email}");
            Console.WriteLine($"Date       : {OrderedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Delivery   : {DeliveryType}");
            Console.WriteLine("---------------------------");
            foreach (var item in Items)
            {
                Console.WriteLine($"  {item.Product.Name} x{item.Count} = {item.SubTotal} AZN");
            }
            Console.WriteLine("---------------------------");
            Console.WriteLine($"  Subtotal   : {Total - DeliveryFee} AZN");
            if (DiscountPercent > 0)
                Console.WriteLine($"  Discount   : -{Discount} AZN ({DiscountPercent}%)");
            Console.WriteLine($"  Delivery   : {DeliveryFee} AZN");
            Console.WriteLine($"  TOTAL      : {FinalTotal} AZN");
            Console.WriteLine($"  STATUS     : {Status}");
            Console.WriteLine("===========================\n");
        }
    }
}
