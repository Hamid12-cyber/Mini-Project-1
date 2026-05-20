using Mini_Project_1.AbstractClasses;
using Mini_Project_1.Enums;

namespace Mini_Project_1.Models
{
    internal class Product : BaseEntity
    {
        private string _name;
        private decimal _price;
        private int _stock;
        private static int _idCounter = 1;

        private static readonly System.Globalization.CultureInfo AzCulture =
            new System.Globalization.CultureInfo("az-Latn-AZ");

        public string Name
        {
            get { return _name; }
            set
            {
                if (value.Trim().Length < 1)
                    Console.WriteLine("Mehsulun adi en azi 1 simvol olmalidir.");
                string trimmed = value.Trim();
                _name = trimmed.Substring(0, 1).ToUpper(AzCulture) + trimmed.Substring(1);
            }
        }

        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value <= 0) Console.WriteLine("Qiymet sifirdan boyuk olmalidir.");
                _price = value;
            }
        }

        public int Stock
        {
            get { return _stock; }
            set
            {
                if (value < 0) Console.WriteLine("Yanlis reqem daxil etdiniz.");
                _stock = value;
            }
        }

        public int Id { get; set; }
        public ProductCategory Category { get; set; }

        public Product() { }

        public Product(string name, decimal price, int stock, ProductCategory category)
        {
            Id = _idCounter++;
            Name = name;
            Price = price;
            Stock = stock;
            Category = category;
        }

        public static void SyncCounter(int maxExistingId)
        {
            if (maxExistingId >= _idCounter)
                _idCounter = maxExistingId + 1;
        }

        public static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ┌───────┬─────────────────────────┬───────────────┬───────────┬──────────────┐");
            Console.WriteLine("  │ ID    │ Məhsul Adı              │ Qiymət        │ Stok      │ Kateqoriya   │");
            Console.WriteLine("  ├───────┼─────────────────────────┼───────────────┼───────────┼──────────────┤");
            Console.ResetColor();
        }

        public override void PrintInfo()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  │ {0,-5} │ {1,-23} │ {2,-13} │ {3,-9} │ {4,-12} │",
                Id,
                Name,
                Price.ToString("0.00") + " AZN",
                Stock,
                Category);
            Console.ResetColor();
        }

        public static void PrintFooter()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  └───────┴─────────────────────────┴───────────────┴───────────┴──────────────┘");
            Console.ResetColor();
        }
    }
}