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
            get { return Items.Sum(item => item.SubTotal) + DeliveryFee; }
        }

        public decimal DiscountPercent
        {
            get
            {
                decimal ps = Items.Sum(item => item.SubTotal);
                if (ps >= 300) return 10;
                if (ps >= 100) return 5;
                return 0;
            }
        }

        public decimal Discount
        {
            get
            {
                decimal ps = Items.Sum(item => item.SubTotal);
                return Math.Round(ps * DiscountPercent / 100, 2);
            }
        }

        public decimal FinalTotal => Total - Discount;

        public string Email
        {
            get { return _email; }
            set
            {
                if (!value.Contains('@'))
                    Console.WriteLine("Emailde @ isaresi vacibdir.");
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

        private static void Row(string label, string value, ConsoleColor valColor = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  | {label,-14} ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(": ");
            Console.ForegroundColor = valColor;
            Console.WriteLine($"{value,-36} |");
            Console.ResetColor();
        }

        private static void Divider()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +----------------+--------------------------------------+");
            Console.ResetColor();
        }

        private static void TopBot(bool top)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(top
                ? "  +================+======================================+"
                : "  +================+======================================+");
            Console.ResetColor();
        }

        public override void PrintInfo()
        {
            TopBot(true);
            Row("Sifaris No", $"#{Id}", ConsoleColor.Yellow);
            Divider();
            Row("Email", Email);
            Row("Status", Status.ToString(), ConsoleColor.Yellow);
            Row("Tarix", OrderedAt.ToString("dd.MM.yyyy HH:mm"));
            Row("Catdirilma", $"{DeliveryType} ({DeliveryFee} AZN)");
            Divider();
            Row("Mehsul cemi", $"{Total - DeliveryFee} AZN");
            if (DiscountPercent > 0)
                Row($"Endirim {DiscountPercent}%", $"-{Discount} AZN", ConsoleColor.Yellow);
            Row("Catdirilma", $"{DeliveryFee} AZN");
            Row("YEKUN", $"{FinalTotal} AZN", ConsoleColor.Yellow);
            TopBot(false);
        }

        public void PrintReceipt()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================================+");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  |              *** RECEIPT ***                   |");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================================+");
            Console.ResetColor();

            Row("Order No", $"#{Id}", ConsoleColor.Yellow);
            Row("Email", Email);
            Row("Tarix", OrderedAt.ToString("dd.MM.yyyy HH:mm"));
            Row("Catdirilma", DeliveryType);

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +--------+------------------------+-----+-----------+");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  | No     | Mehsul                 | Say | Mebleg    |");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +--------+------------------------+-----+-----------+");
            Console.ResetColor();

            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("  | #{0,-5} | {1,-22} | {2,-3} | {3,-9} |",
                    i + 1,
                    item.Product.Name,
                    item.Count,
                    item.SubTotal.ToString("0.00") + " AZN");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +--------+------------------------+-----+-----------+");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================================+");
            Console.ResetColor();

            Row("Subtotal", $"{Total - DeliveryFee:0.00} AZN");

            if (DiscountPercent > 0)
                Row($"Endirim {DiscountPercent}%", $"-{Discount:0.00} AZN", ConsoleColor.Yellow);

            Row("Catdirilma", $"{DeliveryFee:0.00} AZN");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +------------------------------------------------+");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  | TOTAL          : {0,-34}|", $"{FinalTotal:0.00} AZN");
            Console.WriteLine("  | STATUS         : {0,-34}|", Status.ToString());
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("  +================================================+");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
